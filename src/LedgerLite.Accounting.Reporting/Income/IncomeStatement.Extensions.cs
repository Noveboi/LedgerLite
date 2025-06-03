using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Accounts.Metadata;

namespace LedgerLite.Accounting.Reporting.Income;

internal static class IncomeStatementExtensions
{
    public static decimal GetGrossProfitMargin(this IncomeStatement statement)
    {
        return 1 - statement.DirectExpenses / statement.TotalRevenue;
    }
    
    public static bool IsRevenue(this Account account) => account.Type == AccountType.Revenue;
    public static bool IsExpense(this Account account, ExpenseType type) => account.Type == AccountType.Expense &&
                                                                            account.Metadata.ExpenseType == type;
}