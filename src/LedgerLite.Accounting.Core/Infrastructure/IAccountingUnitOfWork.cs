using LedgerLite.Accounting.Core.Infrastructure.Repositories;
using LedgerLite.SharedKernel.Persistence;

namespace LedgerLite.Accounting.Core.Infrastructure;

public interface IAccountingUnitOfWork : IUnitOfWork
{
    IAccountRepository AccountRepository { get; }
    IChartOfAccountsRepository ChartOfAccountsRepository { get; }
    IJournalEntryRepository JournalEntryRepository { get; }
    IFiscalPeriodRepository FiscalPeriodRepository { get; }
}