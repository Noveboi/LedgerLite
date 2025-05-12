using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using Assembly = System.Reflection.Assembly;

namespace LedgerLite.ArchitectureTests;

internal sealed class LedgerLiteArchitectureRules
{
    private readonly GivenTypesConjunctionWithDescription _domain;
    private readonly GivenTypesConjunctionWithDescription _application;
    private readonly GivenTypesConjunctionWithDescription _infrastructure;
    private readonly GivenTypesConjunctionWithDescription _endpoints;

    private readonly string _moduleName;
    
    public LedgerLiteArchitectureRules(Assembly assembly)
    {
        _moduleName = assembly.GetName().Name!;
        
        _application = Types().That().AreInApplicationLayer(_moduleName).As("Application Types");
        _infrastructure = Types().That().AreInInfrastructureLayer(_moduleName).As("Infrastructure Layer");
        _endpoints = Types().That().AreInEndpointsLayer(_moduleName).As("Endpoint Layer");
        _domain = Types().That().AreInDomainLayer(_moduleName).As("Domain Types");
    }

    public IArchRule DomainLayerHasNoDependencies => _domain
        .Should().NotDependOnAny(_application)
        .AndShould().NotDependOnAny(_infrastructure)
        .AndShould().NotDependOnAny(_endpoints);

    public IArchRule EntitiesShouldBeInDomainLayer =>
        Classes()
            .That()
            .AreDomainEntities()
            .Should()
            .ResideInNamespace(
                RuleBuildingExtensions.GetRegexNamespace(_moduleName, "Domain"), 
                useRegularExpressions: true);

    public static void EntitiesShouldHavePrivateParameterlessConstructor(Architecture arch)
    {
        var badEntities = new List<IType>();
        var isValid = Classes()
            .That()
            .AreDomainEntities()
            .GetObjects(arch)
            .All(entity =>
            {
                var hasParameterlessPrivateCtor = entity
                    .GetConstructors()
                    .Any(ctor => ctor.Visibility == Visibility.Private && !ctor.Parameters.Any());

                if (!hasParameterlessPrivateCtor)
                {
                    badEntities.Add(entity);
                }
                
                return hasParameterlessPrivateCtor;
            });

        if (!isValid)
        {
            throw new Exception(
                $"Entities: {string.Join(", ", badEntities.Select(x => x.Name))} do not have a parameterless private constructor.");
        }
    }
}