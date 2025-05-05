using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.SharedKernel.Persistence;

namespace LedgerLite.Accounting.Core.Application;

public interface IAccountingUnitOfWork : IUnitOfWork
{
    IAccountRepository AccountRepository { get; }
    IChartOfAccountsRepository ChartOfAccountsRepository { get; }
    IJournalEntryRepository JournalEntryRepository { get; }
    IJournalEntryLineRepository JournalEntryLineRepository { get; }
}

public interface IAccountRepository
{
    void Add(Account account);
    void Remove(Account account);
    
    Task<Account?> GetByIdAsync(Guid id, CancellationToken token);
}