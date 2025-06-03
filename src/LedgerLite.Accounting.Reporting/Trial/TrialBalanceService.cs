using Ardalis.Result;
using LedgerLite.Accounting.Core.Infrastructure;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LedgerLite.Accounting.Reporting.Trial;

internal sealed class TrialBalanceService(AccountingDbContext context)
{
    private readonly ILogger _log = Log.ForContext<TrialBalanceService>();

    public async Task<Result<TrialBalance>> CreateTrialBalanceAsync(
        CreateTrialBalanceRequest request,
        CancellationToken token)
    {
        var entries = await context.JournalEntries
            .AsNoTracking()
            .IncludeAccounts()
            .Where(x => x.FiscalPeriodId == request.Period.Id)
            .ToListAsync(token);

        _log.Information(messageTemplate: "Found {entryCount} journal entries associated with period.", entries.Count);
        return TrialBalance.Prepare(period: request.Period, journalEntries: entries);
    }

    public async Task<Result<TrialBalance>> CreateTrialBalanceAsync(
        CreateTrialBalanceDateRequest request,
        CancellationToken token)
    {
        var lines = await context.JournalEntries
            .AsNoTracking()
            .IncludeAccounts()
            .Where(x => x.OccursAt >= request.DateRange.Start &&
                        x.OccursAt <= request.DateRange.End)
            .SelectMany(x => x.Lines)
            .ToListAsync(token);
        
        _log.Information("Found {entryCount} journal entry lines between {@range}", lines, request.DateRange);
        return TrialBalance.Prepare(lines);
    }
}