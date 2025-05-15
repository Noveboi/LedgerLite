using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.Accounting.Core.Application.Accounts;
using LedgerLite.Accounting.Core.Application.Accounts.Requests;
using LedgerLite.Accounting.Core.Application.Chart;
using LedgerLite.SharedKernel.Identity;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts;

internal sealed record MoveAccountRequestDto(
    [property: FromClaim(LedgerClaims.UserId)] Guid UserId,
    [property: RouteParam] Guid AccountId, 
    [property: RouteParam] Guid NewParentId);

internal sealed class MoveAccountEndpoint(IChartOfAccountsService charts, IAccountService accounts) : Endpoint<MoveAccountRequestDto>
{
    public override void Configure()
    {
        Put("move/{newParentId:guid}");
        Group<AccountEndpointsGroup>();
    }

    public override async Task HandleAsync(MoveAccountRequestDto req, CancellationToken ct)
    {
        var chart = await charts.GetByUserIdAsync(req.UserId, ct);
        if (!chart.IsSuccess)
        {
            await SendResultAsync(chart.ToMinimalApiResult());
            return;
        }
        
        var request = new MoveAccountRequest(chart, req.AccountId, req.NewParentId);
        var result = await accounts.MoveAsync(request, ct);
        await SendResultAsync(result.ToMinimalApiResult());
    }
}