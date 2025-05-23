using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;
using LedgerLite.SharedKernel.Domain.Errors;
using Serilog;

namespace LedgerLite.Accounting.Reporting.Trial;

internal sealed class TrialBalanceService(
    IFiscalPeriodRepository fiscalPeriodRepository,
    IJournalEntryRepository journalEntryService)
{
    private readonly ILogger _log = Log.ForContext<TrialBalanceService>();
    
    public async Task<Result<TrialBalance>> CreateTrialBalanceAsync(CreateTrialBalanceRequest request, CancellationToken token)
    {
        if (await fiscalPeriodRepository.GetByIdAsync(request.FiscalPeriodId, token) is not { } fiscalPeriod)
        {
            return Result.NotFound(CommonErrors.NotFound<FiscalPeriod>(request.FiscalPeriodId));
        }
        
        _log.Information("Preparing trial balance for period {period}", fiscalPeriod.Id);
        var entries = await journalEntryService.GetByFiscalPeriodIdAsync(fiscalPeriod.Id, token);
        
        _log.Information("Found {entryCount} journal entries associated with period.", entries.Count);
        return TrialBalance.Prepare(fiscalPeriod, entries);
    }
}