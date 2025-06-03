using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.SharedKernel.Models;

namespace LedgerLite.Accounting.Reporting.Trial;

internal sealed record CreateTrialBalanceRequest(FiscalPeriod Period);
internal sealed record CreateTrialBalanceDateRequest(DateRange DateRange);