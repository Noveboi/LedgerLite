using FastEndpoints;
using LedgerLite.Accounting;
using LedgerLite.Accounting.Core;
using LedgerLite.Users;

namespace LedgerLite.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddModules(this IServiceCollection services) => services
        .AddUsersModule()
        .AddAccountingModule();

    public static IServiceCollection AddInfrastructure(this IServiceCollection services) => services
        .AddFastEndpoints()
        .AddOpenApi();
}