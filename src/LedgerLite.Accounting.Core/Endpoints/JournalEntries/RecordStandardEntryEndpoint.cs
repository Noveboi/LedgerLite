using System.Security.Claims;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.Accounting.Core.Application.JournalEntries;
using LedgerLite.Accounting.Core.Application.JournalEntries.Requests;
using LedgerLite.Accounting.Core.Endpoints.JournalEntries.Dto;
using LedgerLite.Accounting.Core.Endpoints.JournalEntries.Groups;

namespace LedgerLite.Accounting.Core.Endpoints.JournalEntries;

internal sealed record RecordStandardEntryRequestDto(
    string ReferenceNumber,
    DateOnly OccursAt,
    string Description,
    decimal Amount,
    Guid DebitAccountId,
    Guid CreditAccountId,
    [property: RouteParam] Guid FiscalPeriodId,
    [property: FromClaim(ClaimTypes.NameIdentifier)]
    Guid RequestedByUserId);

internal sealed class RecordStandardEntryEndpoint(ITransactionRecordingService service)
    : Endpoint<RecordStandardEntryRequestDto, JournalEntryResponseDto>
{
    public override void Configure()
    {
        Post("");
        Group<ModifyJournalEntryGroup>();
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
            new { entry.Id },
            JournalEntryResponseDto.FromEntity(entry),
            cancellation: ct);
    }

    private static RecordStandardEntryRequest MapToRequest(RecordStandardEntryRequestDto dto)
    {
        return new RecordStandardEntryRequest(dto.ReferenceNumber,
            dto.OccursAt,
            dto.Description,
            new CreateJournalEntryLineRequest(
                dto.CreditAccountId,
                dto.Amount),
            new CreateJournalEntryLineRequest(
                dto.DebitAccountId,
                dto.Amount),
            dto.RequestedByUserId,
            dto.FiscalPeriodId);
    }
}