using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.Accounting.Core.Application.UseCases;
using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Endpoints.JournalEntries.Groups;
using LedgerLite.Accounting.Core.Infrastructure;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;
using LedgerLite.SharedKernel.Domain.Errors;
using LedgerLite.SharedKernel.Identity;

namespace LedgerLite.Accounting.Core.Endpoints.JournalEntries;

internal sealed record UpdateJournalEntryRequest(
    [property: FromClaim(LedgerClaims.UserId)] Guid UserId,
    [property: RouteParam] Guid FiscalPeriodId,
    [property: RouteParam] Guid LineId,
    decimal? Credit,
    decimal? Debit,
    Guid? TransferAccountId,
    string? Description,
    DateOnly? OccursAt);

internal sealed class UpdateJournalEntryLineEndpoint(
    IAccountingUnitOfWork unitOfWork, 
    PeriodBelongsToUser periodBelongsToUser) : Endpoint<UpdateJournalEntryRequest>
{
    private readonly IJournalEntryLineRepository _lineRepository = unitOfWork.JournalEntryLineRepository;
    private readonly IAccountRepository _accountRepository = unitOfWork.AccountRepository;
    
    public override void Configure()
    {
        Put("/lines/{lineId:guid}");
        Group<ModifyJournalEntryGroup>();
    }

    public override async Task HandleAsync(UpdateJournalEntryRequest req, CancellationToken ct)
    {
        var result = await HandleUseCaseAsync(req, ct);
        if (!result.IsSuccess)
        {
            await SendResultAsync(result.ToMinimalApiResult());
            return;
        }

        await SendOkAsync(ct);
    }

    public async Task<Result> HandleUseCaseAsync(UpdateJournalEntryRequest req, CancellationToken ct)
    {
        return await periodBelongsToUser.HandleAsync(new DoesPeriodBelongToUser(req.UserId, req.FiscalPeriodId), ct)
            .BindAsync(async _ => req.TransferAccountId is { } accountId
                ? await _accountRepository.ExistsAsync(accountId, ct)
                    ? Result.Success()
                    : Result.NotFound(CommonErrors.NotFound<Account>(accountId))
                : Result.Success())
            .BindAsync(async _ => await _lineRepository.GetByIdAsync(req.LineId, req.FiscalPeriodId, ct) is { } line
                ? Result.Success(line)
                : Result.NotFound(CommonErrors.NotFound<JournalEntryLine>(req.LineId)))
            .BindAsync(line => TransactionTypes.GetTransactionType(req.Credit, req.Debit)
                .Map(tuple => new { Line = line, tuple.Type, tuple.Amount}))
            .BindAsync(state => state.Line.Entry.Update(
                userId: req.UserId,
                description: req.Description,
                occursAt: req.OccursAt,
                lineRequest: new UpdateLineRequest(
                    LineId: req.LineId,
                    Type: state.Type,
                    Amount: state.Amount,
                    TransferAccountId: req.TransferAccountId)))
            .BindAsync(_ => unitOfWork.SaveChangesAsync(ct));
    }
}