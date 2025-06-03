using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.Accounting.Core.Application.Chart;
using LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;
using LedgerLite.Accounting.Core.Endpoints.Accounts.Groups;
using LedgerLite.SharedKernel.Identity;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts;

internal sealed record GetChartOfAccountsRequestDto(
    [property: FromClaim(claimType: LedgerClaims.UserId)]
    Guid UserId);

internal sealed class GetChartOfAccountsEndpoint(IChartOfAccountsService chartService)
    : Endpoint<GetChartOfAccountsRequestDto, ChartOfAccountsDto>
{
    public override void Configure()
    {
        Get("");
        Group<ChartOfAccountsEndpointGroup>();
    }

    public override async Task HandleAsync(GetChartOfAccountsRequestDto req, CancellationToken ct)
    {
        var result = await chartService.GetByUserIdAsync(userId: req.UserId, token: ct);
        if (!result.IsSuccess)
        {
            await SendResultAsync(result.ToMinimalApiResult());
            return;
        }

        await SendAsync(ChartOfAccountsDto.FromEntity(chart: result.Value), cancellation: ct);
    }
}