using LedgerLite.Accounting.Core.Domain.Chart;

namespace LedgerLite.Accounting.Core.Application.Accounts.Requests;

public sealed record MoveAccountRequest(ChartOfAccounts Chart, Guid AccountId, Guid ParentId);