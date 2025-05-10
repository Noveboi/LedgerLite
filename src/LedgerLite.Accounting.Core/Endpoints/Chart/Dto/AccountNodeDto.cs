using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;

namespace LedgerLite.Accounting.Core.Endpoints.Chart.Dto;

public sealed record AccountNodeDto(
    SlimAccountDto Account,
    Guid? ParentAccountId)
{
    public static AccountNodeDto FromEntity(AccountNode node) => new(
        Account: SlimAccountDto.FromEntity(node.Account),
        ParentAccountId: node.ParentId);
}