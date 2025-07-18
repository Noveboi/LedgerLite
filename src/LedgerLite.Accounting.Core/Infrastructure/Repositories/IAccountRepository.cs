﻿using LedgerLite.Accounting.Core.Domain.Accounts;

namespace LedgerLite.Accounting.Core.Infrastructure.Repositories;

public interface IAccountRepository
{
    void Add(Account account);
    void Remove(Account account);

    Task<Account?> GetByIdAsync(Guid id, CancellationToken token);
    Task<bool> ExistsAsync(Guid id, CancellationToken token);
}