using LedgerLite.Accounting.Core.Domain.Accounts;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;

public sealed record SlimAccountDto(Guid Id, string Name, string Type, string Currency, bool IsControl)
{
    public static SlimAccountDto FromEntity(Account account) => new(
        Id: account.Id,
        Name: account.Name,
        Type: account.Type.ToString(),
        Currency: account.Currency.ToString(),
        IsControl: account.IsPlaceholder);
}