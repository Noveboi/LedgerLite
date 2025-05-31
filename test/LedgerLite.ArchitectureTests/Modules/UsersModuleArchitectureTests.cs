using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using LedgerLite.Users;
using Assembly = System.Reflection.Assembly;

namespace LedgerLite.ArchitectureTests.Modules;

public class UsersModuleArchitectureTests
{
    private static readonly Assembly Assembly = typeof(IUsersModuleAssemblyMarker).Assembly;

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
    public void EntitiesWithParameterlessPrivateConstructor()
    {
        LedgerLiteArchitectureRules.EntitiesShouldHavePrivateParameterlessConstructor(arch: ModuleArchitecture);
    }
}