using Microsoft.Extensions.DependencyInjection;

namespace LedgerLite.Accounting.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountingModule(this IServiceCollection services)
    {
        return services;
    }
}