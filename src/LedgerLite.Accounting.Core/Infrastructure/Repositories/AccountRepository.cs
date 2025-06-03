using LedgerLite.Accounting.Core.Domain.Accounts;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Accounting.Core.Infrastructure.Repositories;

internal sealed class AccountRepository(AccountingDbContext context) : IAccountRepository
{
    public void Add(Account account)
    {
        context.Accounts.Add(entity: account);
    }

    public void Remove(Account account)
    {
        context.Accounts.Remove(entity: account);
    }

    public Task<Account?> GetByIdAsync(Guid id, CancellationToken token)
    {
        return context.Accounts.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: token);
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken token)
    {
        return context.Accounts.AnyAsync(x => x.Id == id, token);
    }
}