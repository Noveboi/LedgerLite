using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.Accounting.Core.Application.Accounts;
using LedgerLite.Accounting.Core.Application.Accounts.Requests;
using LedgerLite.Accounting.Core.Application.Chart;
using LedgerLite.Accounting.Core.Endpoints.Accounts.Groups;
using LedgerLite.SharedKernel.Identity;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts;

internal sealed record MoveAccountRequestDto(
    [property: FromClaim(claimType: LedgerClaims.UserId)]
    Guid UserId,
    [property: RouteParam] Guid AccountId,
    [property: RouteParam] Guid NewParentId);

internal sealed class MoveAccountEndpoint(IChartOfAccountsService charts, IAccountService accounts)
    : Endpoint<MoveAccountRequestDto>
{
    public override void Configure()
    {
        Put("{accountId:guid}/move/{newParentId:guid}");
        Group<ChartOfAccountsModifyGroup>();
    }

    public override async Task HandleAsync(MoveAccountRequestDto req, CancellationToken ct)
    {
        var chart = await charts.GetByUserIdAsync(userId: req.UserId, token: ct);
        if (!chart.IsSuccess)
        {
            await SendResultAsync(result: chart.ToMinimalApiResult());
            return;
        }

        var request = new MoveAccountRequest(Chart: chart, AccountId: req.AccountId, ParentId: req.NewParentId);
        var result = await accounts.MoveAsync(request: request, token: ct);
        await SendResultAsync(result: result.ToMinimalApiResult());
    }
}