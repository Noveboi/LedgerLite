using System.Security.Claims;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.Accounting.Core.Application.Chart;
using LedgerLite.Accounting.Core.Endpoints.Chart.Dto;
using LedgerLite.Users.Contracts;

namespace LedgerLite.Accounting.Core.Endpoints.Chart;

internal sealed record GetChartOfAccountsRequestDto(
    [property: FromClaim(ClaimTypes.NameIdentifier)] Guid UserId);

internal sealed class GetChartOfAccountsEndpoint(
    IChartOfAccountsService chartService,
    IUsersRequests userRequests) 
    : Endpoint<GetChartOfAccountsRequestDto, ChartOfAccountsDto>
{
    public override void Configure()
    {
        Get("");
        Group<ChartOfAccountsEndpointGroup>();
    }

    public override async Task HandleAsync(GetChartOfAccountsRequestDto req, CancellationToken ct)
    {
        var userResult = await userRequests.GetUserByIdAsync(req.UserId, ct);
        if (!userResult.IsSuccess)
        {
            await SendResultAsync(userResult.ToMinimalApiResult());
            return;
        }

        var user = userResult.Value;
        var chartResult = await chartService.GetByOrganizationIdAsync(user.OrganizationId, ct);
        if (!chartResult.IsSuccess)
        {
            await SendResultAsync(chartResult.ToMinimalApiResult());
            return;
        }

        await SendAsync(ChartOfAccountsDto.FromEntity(chartResult.Value), cancellation: ct);
    }
}