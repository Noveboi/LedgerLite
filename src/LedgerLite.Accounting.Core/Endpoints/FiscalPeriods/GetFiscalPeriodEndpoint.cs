using FastEndpoints;
using LedgerLite.Accounting.Core.Endpoints.FiscalPeriods.Dto;

namespace LedgerLite.Accounting.Core.Endpoints.FiscalPeriods;

internal sealed record GetFiscalPeriodRequestDto([property: RouteParam] Guid Id);
internal sealed class GetFiscalPeriodEndpoint : Endpoint<GetFiscalPeriodRequestDto, FiscalPeriodDto>
{
    public override void Configure()
    {
        Get("{id:guid}");
        Group<FiscalPeriodEndpointGroup>();
    }

    public override Task HandleAsync(GetFiscalPeriodRequestDto req, CancellationToken ct)
    {
        return base.HandleAsync(req, ct);
    }
}