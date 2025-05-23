using System.Diagnostics.CodeAnalysis;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;
using LedgerLite.SharedKernel.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerLite.Accounting.Core.Infrastructure;

internal sealed class AccountingUnitOfWork(IServiceProvider serviceProvider, AccountingDbContext context) 
    : UnitOfWork<AccountingDbContext>(context), IAccountingUnitOfWork
{
    [field: AllowNull, MaybeNull]
    public IAccountRepository AccountRepository => 
        field ?? serviceProvider.GetRequiredService<IAccountRepository>();

    [field: AllowNull, MaybeNull]
    public IChartOfAccountsRepository ChartOfAccountsRepository =>
        field ?? serviceProvider.GetRequiredService<IChartOfAccountsRepository>();

    [field: AllowNull, MaybeNull]
    public IJournalEntryRepository JournalEntryRepository =>
        field ?? serviceProvider.GetRequiredService<IJournalEntryRepository>();
    
    [field: AllowNull, MaybeNull]
    public IJournalEntryLineRepository JournalEntryLineRepository =>
        field ?? serviceProvider.GetRequiredService<IJournalEntryLineRepository>();

    [field: AllowNull, MaybeNull]
    public IFiscalPeriodRepository FiscalPeriodRepository =>
        field ?? serviceProvider.GetRequiredService<IFiscalPeriodRepository>();
}