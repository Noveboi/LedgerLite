using FastEndpoints;
using LedgerLite.Accounting.Core;
using LedgerLite.SharedKernel;
using LedgerLite.Users;
using Serilog;

namespace LedgerLite.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration) => 
        services
            .AddUsersModule(configuration)
            .AddAccountingModule(configuration);

    public static IServiceCollection AddApiInfrastructure(this IServiceCollection services, IConfiguration configuration) => 
        services
            .AddSerilog((_, config) => config.ReadFrom.Configuration(configuration))
            .AddFastEndpoints()
            .AddOpenApi();
}