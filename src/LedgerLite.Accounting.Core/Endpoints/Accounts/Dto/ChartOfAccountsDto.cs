using LedgerLite.Accounting.Core.Domain.Chart;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;

public sealed record ChartOfAccountsDto(
    Guid Id,
    Guid OrganizationId,
    IEnumerable<ChartAccountNodeWithChildrenDto> Accounts)
{
    public static ChartOfAccountsDto FromEntity(ChartOfAccounts chart) => new(
        Id: chart.Id,
        OrganizationId: chart.OrganizationId,
        Accounts: chart.Nodes
            .Where(x => x.ParentId is null)
            .Select(ChartAccountNodeWithChildrenDto.FromEntity));
}