using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.Accounting.Core.Application.UseCases;
using LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;
using LedgerLite.SharedKernel.Identity;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts;

internal sealed record GetAccountRequest(
    [property: FromClaim(LedgerClaims.UserId)] Guid UserId, 
    [property: RouteParam] Guid Id);

internal sealed class GetAccountEndpoint(GetDetailedAccountUseCase useCase) 
    : Endpoint<GetAccountRequest, AccountWithLinesDto>
{
    public override void Configure()
    {
        Get("{id:guid}");
        Group<AccountEndpointsGroup>();
    }

    public override async Task HandleAsync(GetAccountRequest req, CancellationToken ct)
    {
        var request = new GetDetailedAccountRequest(
            UserId: req.UserId,
            AccountId: req.Id);
        
        var result = await useCase.HandleAsync(request, ct);
        if (!result.IsSuccess)
        {
            await SendResultAsync(result.ToMinimalApiResult());
            return;
        }

        await SendAsync(result.Value.ToDto(), cancellation: ct);
    }
}