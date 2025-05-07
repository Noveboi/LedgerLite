using Microsoft.Extensions.DependencyInjection;

namespace LedgerLite.Accounting.Core.Infrastructure;

internal static class DependencyInjection
{
    public static IServiceCollection AddAccountingInfrastructure(this IServiceCollection services)
    {
        return services
            .AddScoped<IAccountingUnitOfWork, AccountingUnitOfWork>()
            .AddScoped<IChartOfAccountsRepository, ChartOfAccountsRepository>()
            .AddScoped<IAccountRepository, AccountRepository>()
            .AddScoped<IJournalEntryRepository, JournalEntryRepository>()
            .AddScoped<IJournalEntryLineRepository, JournalEntryLineRepository>();
    } 
}