using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;
using LedgerLite.Accounting.Core.Endpoints.FiscalPeriods.Dto;

namespace LedgerLite.Accounting.Reporting.Trial.Endpoints;

internal sealed record TrialBalanceDto(
    decimal TotalDebits,
    decimal TotalCredits,
    IEnumerable<AccountBalanceDto> WorkingBalances)
{
    public static TrialBalanceDto FromEntity(TrialBalance entity)
    {
        return new TrialBalanceDto(
            TotalCredits: entity.GetTotalCredits(),
            TotalDebits: entity.GetTotalDebits(),
            WorkingBalances: entity.WorkingBalance.Select(entry => new AccountBalanceDto(
                SlimAccountDto.FromEntity(account: entry.Account),
                entry.Type == TransactionType.Credit ? entry.Amount : 0,
                entry.Type == TransactionType.Debit ? entry.Amount : 0)));
    }
}