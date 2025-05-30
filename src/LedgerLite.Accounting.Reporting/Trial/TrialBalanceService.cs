using Ardalis.Result;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;
using Serilog;

namespace LedgerLite.Accounting.Reporting.Trial;

internal sealed class TrialBalanceService(IJournalEntryRepository journalEntryService)
{
    private readonly ILogger _log = Log.ForContext<TrialBalanceService>();
    
    public async Task<Result<TrialBalance>> CreateTrialBalanceAsync(CreateTrialBalanceRequest request, CancellationToken token)
    {
        var entries = await journalEntryService.GetByFiscalPeriodIdAsync(request.Period.Id, token);
        
        _log.Information("Found {entryCount} journal entries associated with period.", entries.Count);
        return TrialBalance.Prepare(request.Period, entries);
    }
}