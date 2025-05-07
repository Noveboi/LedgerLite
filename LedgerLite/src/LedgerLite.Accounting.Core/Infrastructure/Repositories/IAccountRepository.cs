using LedgerLite.Accounting.Core.Domain.Accounts;

namespace LedgerLite.Accounting.Core.Infrastructure;

public interface IAccountRepository
{
    void Add(Account account);
    void Remove(Account account);
    
    Task<Account?> GetByIdAsync(Guid id, CancellationToken token);
}