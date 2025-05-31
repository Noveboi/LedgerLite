using LedgerLite.Accounting.Core.Domain.Chart;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;

/// <summary>
///     DTO for when an <see cref="AccountNode" /> is part of a <see cref="ChartOfAccounts" /> DTO.
/// </summary>
/// <param name="Account"></param>
/// <param name="ParentAccountId"></param>
public sealed record ChartAccountNodeDto(
    SlimAccountDto Account,
    Guid? ParentAccountId)
{
    public static ChartAccountNodeDto FromEntity(AccountNode node)
    {
        return new ChartAccountNodeDto(
            SlimAccountDto.FromEntity(node.Account),
            node.ParentId);
    }
}