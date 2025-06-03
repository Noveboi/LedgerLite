using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Accounts.Metadata;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Reporting.Trial;
using LedgerLite.SharedKernel.Domain;
using LedgerLite.SharedKernel.Models;

namespace LedgerLite.Accounting.Reporting.Income;

internal sealed class IncomeStatement
{
    private IncomeStatement() {}

    private IncomeStatement(decimal revenue, decimal direct, decimal indirect, decimal interest, decimal tax)
    {
        TotalRevenue = revenue;
        DirectExpenses = direct;
        IndirectExpenses = indirect;
        InterestExpense = interest;
        TaxExpense = tax;
    }
    
    public decimal TotalRevenue { get; }
    public decimal DirectExpenses { get; }
    public decimal IndirectExpenses { get; }
    public decimal InterestExpense { get; }
    public decimal TaxExpense { get; }

    public decimal GrossProfit => TotalRevenue - DirectExpenses;
    public decimal OperatingProfit => GrossProfit - IndirectExpenses;
    public decimal NetProfit => OperatingProfit - InterestExpense - TaxExpense;

    public static IncomeStatement Create(TrialBalance trialBalance)
    {
        return new IncomeStatement(
            revenue: trialBalance.GetTotals(x => x.Account.IsRevenue()),
            direct: trialBalance.GetTotals(x => x.Account.IsExpense(ExpenseType.Direct)),
            indirect: trialBalance.GetTotals(x => x.Account.IsExpense(ExpenseType.Indirect)),
            tax: trialBalance.GetTotals(x => x.Account.IsExpense(ExpenseType.Tax)),
            interest: trialBalance.GetTotals(x => x.Account.IsExpense(ExpenseType.Interest)));
    }
}