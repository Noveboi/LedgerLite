using LedgerLite.Accounting.Core.Domain.Accounts;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;

internal sealed record AccountResponseDto(
    Guid Id,
    string Name,
    string Number,
    string Type,
    string Currency,
    bool IsControl)
{
    public static AccountResponseDto FromEntity(Account account) => new(
        Id: account.Id,
        Name: account.Name,
        Number: account.Number,
        Type: account.Type.ToString(),
        Currency: account.Currency.ToString(),
        IsControl: account.IsPlaceholder);
}

public sealed record SlimAccountDto(Guid Id, string Name, string Type)
{
    public static SlimAccountDto FromEntity(Account account) => new(
        Id: account.Id,
        Name: account.Name,
        Type: account.Type.ToString());
}