using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using Assembly = System.Reflection.Assembly;

namespace LedgerLite.ArchitectureTests;

internal sealed class LedgerLiteArchitectureRules
{
    private readonly GivenTypesConjunctionWithDescription _application;
    private readonly GivenTypesConjunctionWithDescription _domain;
    private readonly GivenTypesConjunctionWithDescription _endpoints;
    private readonly GivenTypesConjunctionWithDescription _infrastructure;

    private readonly string _moduleName;

    public LedgerLiteArchitectureRules(Assembly assembly)
    {
        _moduleName = assembly.GetName().Name!;

        _application = Types().That().AreInApplicationLayer(module: _moduleName).As(description: "Application Types");
        _infrastructure = Types().That().AreInInfrastructureLayer(module: _moduleName)
            .As(description: "Infrastructure Layer");
        _endpoints = Types().That().AreInEndpointsLayer(module: _moduleName).As(description: "Endpoint Layer");
        _domain = Types().That().AreInDomainLayer(module: _moduleName).As(description: "Domain Types");
    }

    public IArchRule DomainLayerHasNoDependencies => _domain
        .Should().NotDependOnAny(types: _application)
        .AndShould().NotDependOnAny(types: _infrastructure)
        .AndShould().NotDependOnAny(types: _endpoints);

    public IArchRule EntitiesShouldBeInDomainLayer =>
        Classes()
            .That()
            .AreDomainEntities()
            .Should()
            .ResideInNamespace(
                pattern: RuleBuildingExtensions.GetRegexNamespace(module: _moduleName, layerName: "Domain"),
                useRegularExpressions: true);

    public static void EntitiesShouldHavePrivateParameterlessConstructor(Architecture arch)
    {
        var badEntities = new List<IType>();
        var isValid = Classes()
            .That()
            .AreDomainEntities()
            .GetObjects(architecture: arch)
            .All(predicate: entity =>
            {
                var hasParameterlessPrivateCtor = entity
                    .GetConstructors()
                    .Any(predicate: ctor => ctor.Visibility == Visibility.Private && !ctor.Parameters.Any());

                if (!hasParameterlessPrivateCtor) badEntities.Add(item: entity);

                return hasParameterlessPrivateCtor;
            });

        if (!isValid)
            throw new Exception(
                message:
                $"Entities: {string.Join(separator: ", ", values: badEntities.Select(selector: x => x.Name))} do not have a parameterless private constructor.");
    }
}