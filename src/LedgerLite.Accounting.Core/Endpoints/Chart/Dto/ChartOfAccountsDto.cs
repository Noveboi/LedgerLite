using LedgerLite.Accounting.Core.Domain.Chart;

namespace LedgerLite.Accounting.Core.Endpoints.Chart.Dto;

public sealed record ChartOfAccountsDto(
    Guid Id,
    Guid OrganizationId,
    IEnumerable<AccountNodeDto> Accounts)
{
    public static ChartOfAccountsDto FromEntity(ChartOfAccounts chart) => new(
        Id: chart.Id,
        OrganizationId: chart.OrganizationId,
        Accounts: chart.Nodes.Select(AccountNodeDto.FromEntity));
}