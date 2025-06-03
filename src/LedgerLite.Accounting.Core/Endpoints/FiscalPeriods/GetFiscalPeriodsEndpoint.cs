using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.Accounting.Core.Application.UseCases;
using LedgerLite.Accounting.Core.Endpoints.FiscalPeriods.Dto;
using LedgerLite.Accounting.Core.Endpoints.FiscalPeriods.Groups;
using LedgerLite.SharedKernel.Identity;

namespace LedgerLite.Accounting.Core.Endpoints.FiscalPeriods;

internal sealed record GetFiscalPeriodsRequest(
    [property: FromClaim(claimType: LedgerClaims.UserId)]
    Guid UserId);

internal sealed class GetFiscalPeriodsEndpoint(GetFiscalPeriodsForUserUseCase getFiscalPeriodsForUser)
    : Endpoint<GetFiscalPeriodsRequest, IEnumerable<FiscalPeriodDto>>
{
    public override void Configure()
    {
        Get("");
        Group<FiscalPeriodEndpointGroup>();
    }

    public override async Task HandleAsync(GetFiscalPeriodsRequest req, CancellationToken ct)
    {
        var request = new GetFiscalPeriodsForUserRequest(UserId: req.UserId);
        var response = await getFiscalPeriodsForUser.HandleAsync(request: request, token: ct);

        if (!response.IsSuccess)
        {
            await SendResultAsync(response.ToMinimalApiResult());
            return;
        }

        await SendAsync(response.Value.Select(x => x.ToDto()), cancellation: ct);
    }
}