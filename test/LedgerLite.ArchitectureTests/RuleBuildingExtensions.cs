using System.Diagnostics;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Classes;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.ArchitectureTests;

internal static class RuleBuildingExtensions
{
    private static readonly Type EntityType = typeof(Entity);

    public static GivenTypesConjunction AreInDomainLayer(
        this GivenTypesThat<GivenTypesConjunction, IType> that,
        string module)
    {
        return that.ResideInNamespace(pattern: GetRegexNamespace(module: module, layerName: "Domain"),
            useRegularExpressions: true);
    }

    public static GivenTypesConjunction AreInApplicationLayer(
        this GivenTypesThat<GivenTypesConjunction, IType> that,
        string module)
    {
        return that.ResideInNamespace(pattern: GetRegexNamespace(module: module, layerName: "Application"),
            useRegularExpressions: true);
    }

    public static GivenTypesConjunction AreInInfrastructureLayer(
        this GivenTypesThat<GivenTypesConjunction, IType> that,
        string module)
    {
        return that.ResideInNamespace(pattern: GetRegexNamespace(module: module, layerName: "Infrastructure"),
            useRegularExpressions: true);
    }

    public static GivenTypesConjunction AreInEndpointsLayer(
        this GivenTypesThat<GivenTypesConjunction, IType> that,
        string module)
    {
        return that.ResideInNamespace(pattern: GetRegexNamespace(module: module, layerName: "Endpoints"),
            useRegularExpressions: true);
    }

    public static GivenClassesConjunctionWithDescription AreDomainEntities(
        this GivenClassesThat that)
    {
        return that.AreAssignableTo(firstType: EntityType).As(description: "Entities");
    }

    public static string GetRegexNamespace(string module, string layerName)
    {
        var @namespace = $@"{module}.{layerName}\..*";
        Debug.Print(message: @namespace);
        return @namespace;
    }
}