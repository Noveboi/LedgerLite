using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using LedgerLite.Accounting.Core;
using Assembly = System.Reflection.Assembly;

namespace LedgerLite.ArchitectureTests.Modules;

public class AccountingModuleArchitectureTests
{
    private static readonly Assembly Assembly = typeof(IAccountingModuleAssemblyMarker).Assembly;

    private static readonly Architecture ModuleArchitecture = new ArchLoader()
        .LoadAssemblies(Assembly)
        .Build();

    private static readonly LedgerLiteArchitectureRules Rules = new(assembly: Assembly);

    [Fact]
    public void DomainDependencies()
    {
        Rules.DomainLayerHasNoDependencies.Check(architecture: ModuleArchitecture);
    }

    [Fact]
    public void EntitiesInDomainLayer()
    {
        Rules.EntitiesShouldBeInDomainLayer.Check(architecture: ModuleArchitecture);
    }

    [Fact]
    public void EntitiesWithParameterlessPrivateConstructor()
    {
        LedgerLiteArchitectureRules.EntitiesShouldHavePrivateParameterlessConstructor(arch: ModuleArchitecture);
    }
}