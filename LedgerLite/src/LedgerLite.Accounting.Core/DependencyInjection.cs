using LedgerLite.Accounting.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerLite.Accounting.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountingModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AccountingDbContext>(options => options
            .UseNpgsql(configuration.GetConnectionString("AccountingDatabase")));

        return services;
    }
}