using LedgerLite.SharedKernel.Persistence;

namespace LedgerLite.Accounting.Core.Infrastructure;

public interface IAccountingUnitOfWork : IUnitOfWork
{
    IChartOfAccountsRepository ChartOfAccountsRepository { get; }
    IJournalEntryRepository JournalEntryRepository { get; }
    IJournalEntryLineRepository JournalEntryLineRepository { get; }
}