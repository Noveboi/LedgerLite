using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.Accounting.Core.Application.Accounts;
using LedgerLite.Accounting.Core.Endpoints.Accounts.Data;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts;

internal sealed record GetAccountRequest([property: RouteParam] Guid Id);
internal sealed class GetAccountEndpoint(IAccountService accountService) : Endpoint<GetAccountRequest, AccountResponseDto>
{
    public override void Configure()
    {
        Get("{id:guid}");
        Group<AccountEndpointsGroup>();
    }

    public override async Task HandleAsync(GetAccountRequest req, CancellationToken ct)
    {
        var result = await accountService.GetAsync(req.Id, ct);
        if (!result.IsSuccess)
        {
            await SendResultAsync(result.ToMinimalApiResult());
            return;
        }

        await SendAsync(AccountResponseDto.FromEntity(result.Value), cancellation: ct);
    }
}