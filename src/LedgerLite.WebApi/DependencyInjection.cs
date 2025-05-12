using FastEndpoints;
using LedgerLite.Accounting.Core;
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

    public static IServiceCollection AddLedgerLiteCors(this IServiceCollection services, IConfiguration configuration) 
    {
        var allowedOrigins = configuration
            .GetRequiredSection("AllowedOrigins")
            .Get<string[]>() ?? throw new InvalidOperationException("API Configuration does not have any allowed origins for CORS.");
        
        return services
            .AddCors(o => o.AddPolicy("LedgerLite", policy => policy
                .WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()));
    }
}