using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.Accounting.Core.Application.UseCases;
using LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;
using LedgerLite.Accounting.Core.Endpoints.Accounts.Groups;
using LedgerLite.SharedKernel.Identity;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts;

internal sealed record RemoveAccountRequestDto(
    [property: FromClaim(LedgerClaims.UserId)]
    Guid UserId,
    [property: RouteParam] Guid AccountId);

internal sealed class RemoveAccountEndpoint(RemoveAccountUseCase removeAccount)
    : Endpoint<RemoveAccountRequestDto, AccountDto>
{
    public override void Configure()
    {
        Delete("{accountId:guid}");
        Group<ChartOfAccountsModifyGroup>();
    }

    public override async Task HandleAsync(RemoveAccountRequestDto req, CancellationToken ct)
    {
        var request = new RemoveAccountRequest(req.UserId, req.AccountId);
        var result = await removeAccount.HandleAsync(request, ct);

        if (!result.IsSuccess)
        {
            await SendResultAsync(result.ToMinimalApiResult());
            return;
        }

        await SendAsync(result.Value.ToDto(), cancellation: ct);
    }
}