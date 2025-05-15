using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Chart;

namespace LedgerLite.Accounting.Core.Application.Accounts.Requests;

public sealed record CreateAccountRequest(
    string Name,
    string Number,
    AccountType Type,
    Currency Currency,
    bool IsPlaceholder,
    string Description,
    ChartOfAccounts Chart,
    Guid? ParentId);