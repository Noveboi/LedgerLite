using LedgerLite.SharedKernel.Constants;
using LedgerLite.SharedKernel.Extensions;
using LedgerLite.SharedKernel.Persistence;
using LedgerLite.Users.Application;
using LedgerLite.Users.Domain;
using LedgerLite.Users.Infrastructure;
using LedgerLite.Users.Integrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LedgerLite.Users;

public static class UsersDependencyInjection
{
    public static IServiceCollection AddLedgerLiteAuth(this IServiceCollection services)
    {
        Log.Information("Registering Authentication/Authorization.");

        services.AddAuthorization();
        services.AddAuthentication(IdentityConstants.BearerScheme)
            .AddCookie(IdentityConstants.ApplicationScheme)
            .AddBearerToken(IdentityConstants.BearerScheme);

        return services;
    }

    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger.RegisteringModule("Users");

        services.AddDbContext<UsersDbContext>((sp, options) => options
            .UseNpgsql(configuration.GetConnectionString(ConnectionStrings.CoreDatabase))
            .AddAuditLogging()
            .AddDomainEventProcessing(sp));

        return services
            .AddUsersIntegrations()
            .AddUsersInfrastructure()
            .AddUsersApplication()
            .AddIdentityCore<User>()
            .AddEntityFrameworkStores<UsersDbContext>()
            .AddApiEndpoints()
            .Services;
    }
}