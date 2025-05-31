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
            .AddUsersModule(configuration)
            .AddAccountingModule(configuration)
            .AddReportingModule(configuration);
    }

    public static IServiceCollection AddApiInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddSerilog((_, config) => config.ReadFrom.Configuration(configuration))
            .AddFastEndpoints()
            .AddOpenApi();
    }

    public static IServiceCollection AddLedgerLiteCors(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration
                                 .GetRequiredSection("AllowedOrigins")
                                 .Get<string[]>() ??
                             throw new InvalidOperationException(
                                 "API Configuration does not have any allowed origins for CORS.");

        return services
            .AddCors(o => o.AddPolicy("LedgerLite", policy => policy
                .WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()));
    }
}