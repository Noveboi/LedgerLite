using LedgerLite.Accounting.Core.Domain.Accounts;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Accounting.Core.Infrastructure.Repositories;

internal sealed class AccountRepository(AccountingDbContext context) : IAccountRepository
{
    public void Add(Account account)
    {
        context.Accounts.Add(account);
    }

    public void Remove(Account account)
    {
        context.Accounts.Remove(account);
    }

    public Task<Account?> GetByIdAsync(Guid id, CancellationToken token)
    {
        return context.Accounts.FirstOrDefaultAsync(x => x.Id == id, token);
    }
}