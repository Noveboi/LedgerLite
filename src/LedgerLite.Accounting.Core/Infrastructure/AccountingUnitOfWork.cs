using System.Diagnostics.CodeAnalysis;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;
using LedgerLite.SharedKernel.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerLite.Accounting.Core.Infrastructure;

internal sealed class AccountingUnitOfWork(IServiceProvider serviceProvider, AccountingDbContext context)
    : UnitOfWork<AccountingDbContext>(context: context), IAccountingUnitOfWork
{
    [field: AllowNull]
    [field: MaybeNull]
    public IAccountRepository AccountRepository =>
        field ?? serviceProvider.GetRequiredService<IAccountRepository>();

    [field: AllowNull]
    [field: MaybeNull]
    public IChartOfAccountsRepository ChartOfAccountsRepository =>
        field ?? serviceProvider.GetRequiredService<IChartOfAccountsRepository>();

    [field: AllowNull]
    [field: MaybeNull]
    public IJournalEntryRepository JournalEntryRepository =>
        field ?? serviceProvider.GetRequiredService<IJournalEntryRepository>();

    [field: AllowNull]
    [field: MaybeNull]
    public IJournalEntryLineRepository JournalEntryLineRepository =>
        field ?? serviceProvider.GetRequiredService<IJournalEntryLineRepository>();

    [field: AllowNull]
    [field: MaybeNull]
    public IFiscalPeriodRepository FiscalPeriodRepository =>
        field ?? serviceProvider.GetRequiredService<IFiscalPeriodRepository>();
}