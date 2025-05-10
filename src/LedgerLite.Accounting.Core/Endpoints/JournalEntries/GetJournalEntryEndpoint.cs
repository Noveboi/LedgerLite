using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.Accounting.Core.Application.JournalEntries;
using LedgerLite.Accounting.Core.Endpoints.JournalEntries.Dto;

namespace LedgerLite.Accounting.Core.Endpoints.JournalEntries;

internal sealed record GetJournalEntryRequest([property: RouteParam] Guid Id);

internal sealed class GetJournalEntryEndpoint(IJournalEntryService service)
    : Endpoint<GetJournalEntryRequest, JournalEntryWithLinesResponseDto>
{
    public override void Configure()
    {
        Get("{id:guid}");
        Group<JournalEntryEndpointGroup>();
    }

    public override async Task HandleAsync(GetJournalEntryRequest req, CancellationToken ct)
    {
        var result = await service.GetByIdAsync(req.Id, ct);
        if (!result.IsSuccess)
        {
            await SendResultAsync(result.ToMinimalApiResult());
            return;
        }

        var entry = result.Value;
        
        await SendAsync(JournalEntryWithLinesResponseDto.FromEntity(entry), cancellation: ct);
    }
}