using FastEndpoints;
using LedgerLite.Accounting.Core.Endpoints.Accounts.Data;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts;

internal sealed class GetAccountEndpoint : EndpointWithoutRequest<AccountResponseDto>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get(AccountSchemes.ResourceUri + "/{id:guid}");
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        return base.HandleAsync(ct);
    }
}