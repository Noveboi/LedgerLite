using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.Accounting.Core.Application.UseCases;
using LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;
using LedgerLite.SharedKernel.Identity;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts;

internal sealed record GetAccountRequest(
    [property: FromClaim(LedgerClaims.UserId)] Guid UserId, 
    [property: RouteParam] Guid AccountId);

internal sealed class GetAccountEndpoint(GetDetailedAccountUseCase useCase) 
    : Endpoint<GetAccountRequest, AccountWithLinesDto>
{
    public override void Configure()
    {
        Get("{accountId:guid}");
        Group<ChartOfAccountsEndpointGroup>();
    }

    public override async Task HandleAsync(GetAccountRequest req, CancellationToken ct)
    {
        var request = new GetDetailedAccountRequest(
            UserId: req.UserId,
            AccountId: req.AccountId);
        
        var result = await useCase.HandleAsync(request, ct);
        if (!result.IsSuccess)
        {
            await SendResultAsync(result.ToMinimalApiResult());
            return;
        }

        await SendAsync(result.Value.ToDto(), cancellation: ct);
    }
}