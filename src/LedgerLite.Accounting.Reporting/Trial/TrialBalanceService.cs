using Ardalis.Result;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;
using Serilog;

namespace LedgerLite.Accounting.Reporting.Trial;

internal sealed class TrialBalanceService(IJournalEntryRepository journalEntryService)
{
    private readonly ILogger _log = Log.ForContext<TrialBalanceService>();

    public async Task<Result<TrialBalance>> CreateTrialBalanceAsync(CreateTrialBalanceRequest request,
        CancellationToken token)
    {
        var entries =
            await journalEntryService.GetByFiscalPeriodIdAsync(fiscalPeriodId: request.Period.Id, token: token);

        _log.Information(messageTemplate: "Found {entryCount} journal entries associated with period.",
            propertyValue: entries.Count);
        return TrialBalance.Prepare(period: request.Period, journalEntries: entries);
    }
}