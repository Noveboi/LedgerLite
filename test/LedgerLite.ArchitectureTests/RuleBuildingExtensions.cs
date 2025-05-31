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
        return that.ResideInNamespace(GetRegexNamespace(module, "Domain"), true);
    }

    public static GivenTypesConjunction AreInApplicationLayer(
        this GivenTypesThat<GivenTypesConjunction, IType> that,
        string module)
    {
        return that.ResideInNamespace(GetRegexNamespace(module, "Application"), true);
    }

    public static GivenTypesConjunction AreInInfrastructureLayer(
        this GivenTypesThat<GivenTypesConjunction, IType> that,
        string module)
    {
        return that.ResideInNamespace(GetRegexNamespace(module, "Infrastructure"), true);
    }

    public static GivenTypesConjunction AreInEndpointsLayer(
        this GivenTypesThat<GivenTypesConjunction, IType> that,
        string module)
    {
        return that.ResideInNamespace(GetRegexNamespace(module, "Endpoints"), true);
    }

    public static GivenClassesConjunctionWithDescription AreDomainEntities(
        this GivenClassesThat that)
    {
        return that.AreAssignableTo(EntityType).As("Entities");
    }

    public static string GetRegexNamespace(string module, string layerName)
    {
        var @namespace = $@"{module}.{layerName}\..*";
        Debug.Print(@namespace);
        return @namespace;
    }
}