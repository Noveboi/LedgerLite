using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.SharedKernel.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LedgerLite.Accounting.Reporting.Trial.Endpoints;

internal sealed record GetTrialBalanceRequest(
    [property: FromClaim(claimType: LedgerClaims.UserId)]
    Guid UserId,
    [property: FromRoute] Guid PeriodId);

internal sealed class GetTrialBalanceEndpoint(
    TrialBalanceService trialBalanceService,
    ReportingUserAuthorization authorization) : Endpoint<GetTrialBalanceRequest, TrialBalanceDto>
{
    public override void Configure()
    {
        Get("");
        Group<TrialBalanceEndpointGroup>();
    }

    public override async Task HandleAsync(GetTrialBalanceRequest req, CancellationToken ct)
    {
        var authorizationResult =
            await authorization.AuthorizeAsync(userId: req.UserId, fiscalPeriodId: req.PeriodId, ct: ct);
        if (!authorizationResult.IsSuccess)
        {
            await SendResultAsync(authorizationResult.ToMinimalApiResult());
            return;
        }

        var fiscalPeriod = authorizationResult.Value;

        var request = new CreateTrialBalanceRequest(Period: fiscalPeriod);
        var trialBalanceResult = await trialBalanceService.CreateTrialBalanceAsync(request: request, token: ct);
        if (!trialBalanceResult.IsSuccess)
        {
            await SendResultAsync(trialBalanceResult.ToMinimalApiResult());
            return;
        }

        await SendAsync(TrialBalanceDto.FromEntity(entity: trialBalanceResult.Value), cancellation: ct);
    }
}