using FastEndpoints;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Endpoints.JournalEntries.Dto;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;
using LedgerLite.SharedKernel.Domain.Errors;
using Microsoft.AspNetCore.Http;

namespace LedgerLite.Accounting.Core.Endpoints.JournalEntries;

internal sealed record GetJournalEntryRequest([property: RouteParam] Guid Id);

internal sealed class GetJournalEntryEndpoint(IJournalEntryRepository repository)
    : Endpoint<GetJournalEntryRequest, JournalEntryWithLinesResponseDto>
{
    public override void Configure()
    {
        Get("{id:guid}");
        Group<JournalEntryEndpointGroup>();
    }

    public override async Task HandleAsync(GetJournalEntryRequest req, CancellationToken ct)
    {
        if (await repository.GetByIdAsync(req.Id, ct) is not { } journalEntry)
        {
            await SendResultAsync(Results.NotFound(CommonErrors.NotFound<JournalEntry>(req.Id)));
            return;
        }
        
        await SendAsync(JournalEntryWithLinesResponseDto.FromEntity(journalEntry), cancellation: ct);
    }
}