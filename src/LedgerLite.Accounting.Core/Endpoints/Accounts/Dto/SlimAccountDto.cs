using LedgerLite.Accounting.Core.Domain.Accounts;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;

public sealed record SlimAccountDto(Guid Id, string Name, string Type, string Currency, bool IsControl)
{
    public static SlimAccountDto FromEntity(Account account)
    {
        return new SlimAccountDto(
            account.Id,
            account.Name,
            account.Type.ToString(),
            account.Currency.ToString(),
            account.IsPlaceholder);
    }
}