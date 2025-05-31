using LedgerLite.Accounting.Core.Domain.Chart;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;

public sealed record ChartOfAccountsDto(
    Guid Id,
    Guid OrganizationId,
    IEnumerable<ChartAccountNodeWithChildrenDto> Accounts)
{
    public static ChartOfAccountsDto FromEntity(ChartOfAccounts chart)
    {
        return new ChartOfAccountsDto(
            Id: chart.Id,
            OrganizationId: chart.OrganizationId,
            Accounts: chart.Nodes
                .Where(predicate: x => x.ParentId is null)
                .Select(selector: ChartAccountNodeWithChildrenDto.FromEntity));
    }
}