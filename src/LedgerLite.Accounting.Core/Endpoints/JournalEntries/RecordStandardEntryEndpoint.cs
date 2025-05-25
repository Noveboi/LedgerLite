using System.Security.Claims;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.Accounting.Core.Application.JournalEntries;
using LedgerLite.Accounting.Core.Application.JournalEntries.Requests;
using LedgerLite.Accounting.Core.Endpoints.JournalEntries.Dto;

namespace LedgerLite.Accounting.Core.Endpoints.JournalEntries;

internal sealed record RecordStandardEntryRequestDto(
    string ReferenceNumber,
    DateOnly OccursAt,
    string Description,
    decimal Amount,
    Guid DebitAccountId,
    Guid CreditAccountId,
    [property: RouteParam] Guid FiscalPeriodId,
    [property: FromClaim(ClaimTypes.NameIdentifier)] Guid RequestedByUserId);

internal sealed class RecordStandardEntryEndpoint(ITransactionRecordingService service) 
    : Endpoint<RecordStandardEntryRequestDto, JournalEntryResponseDto>
{
    public override void Configure()
    {
        Post("");
        Group<JournalEntryEndpointGroup>();
    }

    public override async Task HandleAsync(RecordStandardEntryRequestDto req, CancellationToken ct)
    {
        var request = MapToRequest(req);
        var result = await service.RecordStandardEntryAsync(request, ct);

        if (!result.IsSuccess)
        {
            await SendResultAsync(result.ToMinimalApiResult());
            return;
        }

        var entry = result.Value;

        await SendCreatedAtAsync<GetJournalEntryEndpoint>(
            routeValues: new { entry.Id },
            responseBody: JournalEntryResponseDto.FromEntity(entry),
            cancellation: ct);
    }

    private static RecordStandardEntryRequest MapToRequest(RecordStandardEntryRequestDto dto) => 
        new(ReferenceNumber: dto.ReferenceNumber,
            OccursAt: dto.OccursAt,
            Description: dto.Description,
            CreditLine: new CreateJournalEntryLineRequest(
                AccountId: dto.CreditAccountId,
                Amount: dto.Amount),
            DebitLine: new CreateJournalEntryLineRequest(
                AccountId: dto.DebitAccountId,
                Amount: dto.Amount),
            RequestedByUserId: dto.RequestedByUserId,
            FiscalPeriodId: dto.FiscalPeriodId);
}