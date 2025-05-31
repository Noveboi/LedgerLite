using LedgerLite.Accounting.Core.Domain.Chart;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;

public sealed record ChartAccountNodeWithChildrenDto(
    SlimAccountDto Account,
    IEnumerable<ChartAccountNodeWithChildrenDto> Children)
{
    public static ChartAccountNodeWithChildrenDto FromEntity(AccountNode node)
    {
        return new ChartAccountNodeWithChildrenDto(
            SlimAccountDto.FromEntity(node.Account),
            node.Children.Select(FromEntity));
    }
}