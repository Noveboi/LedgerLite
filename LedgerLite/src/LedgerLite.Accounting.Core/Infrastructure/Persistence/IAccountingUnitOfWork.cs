using LedgerLite.SharedKernel.Persistence;

namespace LedgerLite.Accounting.Core.Infrastructure.Persistence;

public interface IAccountingUnitOfWork : IUnitOfWork
{
    IChartOfAccountsRepository ChartOfAccountsRepository { get; }
    IJournalEntryRepository JournalEntryRepository { get; }
    IJournalEntryLineRepository JournalEntryLineRepository { get; }
}