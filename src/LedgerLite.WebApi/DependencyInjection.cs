using FastEndpoints;
using LedgerLite.Accounting.Core;
using LedgerLite.Accounting.Reporting;
using LedgerLite.Users;
using Serilog;

namespace LedgerLite.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddUsersModule(configuration: configuration)
            .AddAccountingModule(configuration: configuration)
            .AddReportingModule(configuration: configuration);
    }

    public static IServiceCollection AddApiInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddSerilog(configureLogger: (_, config) => config.ReadFrom.Configuration(configuration: configuration))
            .AddFastEndpoints()
            .AddOpenApi();
    }

    public static IServiceCollection AddLedgerLiteCors(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration
                                 .GetRequiredSection(key: "AllowedOrigins")
                                 .Get<string[]>() ??
                             throw new InvalidOperationException(
                                 message: "API Configuration does not have any allowed origins for CORS.");

        return services
            .AddCors(setupAction: o => o.AddPolicy(name: "LedgerLite", configurePolicy: policy => policy
                .WithOrigins(origins: allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()));
    }
}