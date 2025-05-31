using LedgerLite.Accounting.Core.Domain.Chart;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;

/// <summary>
///     DTO for when an <see cref="AccountNode" /> is part of a <see cref="ChartOfAccounts" /> DTO.
/// </summary>
public sealed record ChartAccountNodeDto(
    SlimAccountDto Account,
    Guid? ParentAccountId)
{
    public static ChartAccountNodeDto FromEntity(AccountNode node)
    {
        return new ChartAccountNodeDto(
            Account: SlimAccountDto.FromEntity(account: node.Account),
            ParentAccountId: node.ParentId);
    }
}