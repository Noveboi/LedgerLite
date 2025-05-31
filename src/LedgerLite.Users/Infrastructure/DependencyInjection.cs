using LedgerLite.Users.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerLite.Users.Infrastructure;

internal static class DependencyInjection
{
    public static IServiceCollection AddUsersInfrastructure(this IServiceCollection services)
    {
        return services
            .AddScoped<IUsersUnitOfWork, UsersUnitOfWork>()
            .AddScoped<IOrganizationRepository, OrganizationRepository>();
    }
}