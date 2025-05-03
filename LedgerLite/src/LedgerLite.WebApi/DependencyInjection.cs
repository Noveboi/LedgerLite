using FastEndpoints;
using LedgerLite.Accounting.Core;
using LedgerLite.Users;

namespace LedgerLite.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration) => 
        services
            .AddUsersModule()
            .AddAccountingModule(configuration);

    public static IServiceCollection AddApiInfrastructure(this IServiceCollection services) => services
        .AddFastEndpoints()
        .AddOpenApi();
}