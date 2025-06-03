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
        Log.Information(messageTemplate: "Registering Authentication/Authorization.");

        services.AddAuthorization();
        services.AddAuthentication(defaultScheme: IdentityConstants.BearerScheme)
            .AddCookie(authenticationScheme: IdentityConstants.ApplicationScheme)
            .AddBearerToken(authenticationScheme: IdentityConstants.BearerScheme);

        return services;
    }

    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger.RegisteringModule(moduleName: "Users");

        services.AddDbContext<UsersDbContext>((sp, options) => options
            .UseNpgsql(configuration.GetConnectionString(name: ConnectionStrings.CoreDatabase))
            .AddAuditLogging()
            .AddDomainEventProcessing(sp: sp));

        return services
            .AddUsersIntegrations()
            .AddUsersInfrastructure()
            .AddUsersApplication()
            .AddIdentityCore<User>()
            .AddRoles<Role>()
            .AddEntityFrameworkStores<UsersDbContext>()
            .AddApiEndpoints()
            .Services;
    }
}