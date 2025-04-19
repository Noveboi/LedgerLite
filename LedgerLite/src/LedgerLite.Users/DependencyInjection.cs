using Microsoft.Extensions.DependencyInjection;

namespace LedgerLite.Users;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services)
    {
        return services;
    } 
}