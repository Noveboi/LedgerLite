using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.Accounts;

namespace LedgerLite.Accounting.Core.Application.Accounts.Requests;

public sealed record CreateAccountRequest(
    string Name,
    string Number,
    AccountType Type,
    Currency Currency,
    bool IsPlaceholder,
    string Description,
    Guid ChartOfAccountsId,
    Guid? ParentId);