using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;
using LedgerLite.Accounting.Core.Endpoints.FiscalPeriods.Dto;

namespace LedgerLite.Accounting.Reporting.Trial.Endpoints;

internal sealed record TrialBalanceDto(
    decimal TotalDebits,
    decimal TotalCredits,
    FiscalPeriodDto Period,
    IEnumerable<AccountBalanceDto> WorkingBalances)
{
    public static TrialBalanceDto FromEntity(TrialBalance entity) => new(
        TotalCredits: entity.GetTotalCredits(),
        TotalDebits: entity.GetTotalDebits(),
        Period: FiscalPeriodDto.FromEntity(entity.Period),
        WorkingBalances: entity.WorkingBalance.Select(entry => new AccountBalanceDto(
            Account: SlimAccountDto.FromEntity(entry.Account),
            Credit: entry.Type == TransactionType.Credit ? entry.Amount : 0,
            Debit: entry.Type == TransactionType.Debit ? entry.Amount : 0)));
}