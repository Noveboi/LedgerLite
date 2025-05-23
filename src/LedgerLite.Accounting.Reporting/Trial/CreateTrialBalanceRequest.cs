using LedgerLite.Accounting.Core.Domain.Periods;

namespace LedgerLite.Accounting.Reporting.Trial;

internal sealed record CreateTrialBalanceRequest(FiscalPeriod Period);