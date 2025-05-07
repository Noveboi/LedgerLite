using LedgerLite.Accounting.Core.Domain.Accounts;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts.Data;

internal sealed record AccountResponseDto(
    string Name,
    string Number,
    string Type,
    string Currency,
    bool IsControl)
{
    public static AccountResponseDto FromEntity(Account account) => new(
        Name: account.Name,
        Number: account.Number,
        Type: account.Type.ToString(),
        Currency: account.Currency.ToString(),
        IsControl: account.IsPlaceholder);
}