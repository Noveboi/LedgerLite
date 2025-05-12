using LedgerLite.Accounting.Core.Application.Accounts;
using LedgerLite.Accounting.Core.Application.Chart;
using LedgerLite.Accounting.Core.Application.FiscalPeriods;
using LedgerLite.Accounting.Core.Application.JournalEntries;
using LedgerLite.Accounting.Core.Infrastructure;
using LedgerLite.SharedKernel.Constants;
using LedgerLite.SharedKernel.Extensions;
using LedgerLite.SharedKernel.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LedgerLite.Accounting.Core;

public static class AccountingDependencyInjection
{
    public static IServiceCollection AddAccountingModule(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger.RegisteringModule("Accounting");

        services.AddDbContext<AccountingDbContext>((sp, options) => options
            .UseNpgsql(configuration.GetConnectionString(ConnectionStrings.CoreDatabase))
            .AddAuditLogging()
            .AddDomainEventProcessing(sp));

        services.AddAccountingInfrastructure();

        return services
            .AddScoped<IFiscalPeriodService, FiscalPeriodService>()
            .AddScoped<IChartOfAccountsService, ChartOfAccountsService>()
            .AddScoped<IAccountService, AccountService>()
            .AddScoped<IJournalEntryService, JournalEntryService>()
            .AddScoped<ITransactionRecordingService, TransactionRecordingService>();
    }
}