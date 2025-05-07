using LedgerLite.Accounting.Core.Application.Accounts;
using LedgerLite.Accounting.Core.Application.JournalEntries;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerLite.Accounting.Core.Application;

internal static class DependencyInjection
{
    public static IServiceCollection AddAccountingApplication(this IServiceCollection services)
    {
        return services
            .AddScoped<IAccountService, AccountService>()
            .AddScoped<ITransactionRecordingService, TransactionRecordingService>();
    }
}