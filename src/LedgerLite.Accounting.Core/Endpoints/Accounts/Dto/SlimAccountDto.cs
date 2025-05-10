using LedgerLite.Accounting.Core.Domain.Accounts;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;

public sealed record SlimAccountDto(Guid Id, string Name, string Type)
{
    public static SlimAccountDto FromEntity(Account account) => new(
        Id: account.Id,
        Name: account.Name,
        Type: account.Type.ToString());
}