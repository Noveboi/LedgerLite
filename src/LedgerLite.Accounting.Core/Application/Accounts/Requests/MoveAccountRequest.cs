namespace LedgerLite.Accounting.Core.Application.Accounts.Requests;

public sealed record MoveAccountRequest(Guid AccountId, Guid ParentId);