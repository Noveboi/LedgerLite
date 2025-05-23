using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

namespace LedgerLite.Accounting.Reporting.Tests.Unit;

internal static class CommonAccounts
{
    // Assets
    public static readonly Account Cash = FakeAccounts.Get(x => x.Type = AccountType.Asset);
    public static readonly Account Supplies = FakeAccounts.Get(x => x.Type = AccountType.Asset);
    public static readonly Account Equipment = FakeAccounts.Get(x => x.Type = AccountType.Asset);
    
    // Liabilities
    public static readonly Account AccountsPayable = FakeAccounts.Get(x => x.Type = AccountType.Liability);
    public static readonly Account LoansPayable = FakeAccounts.Get(x => x.Type = AccountType.Liability);
    
    // Equity
    public static readonly Account OwnerEquity = FakeAccounts.Get(x => x.Type = AccountType.Equity);
    
    // Revenue
    public static readonly Account Revenue = FakeAccounts.Get(x => x.Type = AccountType.Revenue);
    
    // Expenses
    public static readonly Account CostOfSales = FakeAccounts.Get(x => x.Type = AccountType.Expense);
    public static readonly Account LaundryCosts = FakeAccounts.Get(x => x.Type = AccountType.Expense);
}