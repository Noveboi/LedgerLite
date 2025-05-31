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
    [property: FromClaim(claimType: ClaimTypes.NameIdentifier)]
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
        var request = MapToRequest(dto: req);
        var result = await service.RecordStandardEntryAsync(req: request, ct: ct);

        if (!result.IsSuccess)
        {
            await SendResultAsync(result: result.ToMinimalApiResult());
            return;
        }

        var entry = result.Value;

        await SendCreatedAtAsync<GetJournalEntryEndpoint>(
            routeValues: new { entry.Id },
            responseBody: JournalEntryResponseDto.FromEntity(entry: entry),
            cancellation: ct);
    }

    private static RecordStandardEntryRequest MapToRequest(RecordStandardEntryRequestDto dto)
    {
        return new RecordStandardEntryRequest(ReferenceNumber: dto.ReferenceNumber,
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
}