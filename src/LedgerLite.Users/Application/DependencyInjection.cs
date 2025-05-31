using LedgerLite.Users.Application.Roles;
using LedgerLite.Users.Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerLite.Users.Application;

internal static class DependencyInjection
{
    public static IServiceCollection AddUsersApplication(this IServiceCollection services)
    {
        return services
            .AddScoped<RoleMaker>()
            .AddScoped<IRoleService, RoleService>()
            .AddScoped<IUserService, UserService>();
    }
}