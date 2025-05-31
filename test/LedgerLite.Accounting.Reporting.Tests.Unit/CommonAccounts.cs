using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

namespace LedgerLite.Accounting.Reporting.Tests.Unit;

internal static class CommonAccounts
{
    // Assets
    public static readonly Account Cash = FakeAccounts.Get(x => x.WithName("Cash", AccountType.Asset));
    public static readonly Account Supplies = FakeAccounts.Get(x => x.WithName("Supplies", AccountType.Asset));
    public static readonly Account Equipment = FakeAccounts.Get(x => x.WithName("Equipment", AccountType.Asset));

    // Liabilities
    public static readonly Account AccountsPayable =
        FakeAccounts.Get(x => x.WithName("Accounts Payable", AccountType.Liability));

    public static readonly Account LoansPayable =
        FakeAccounts.Get(x => x.WithName("Loans Payable", AccountType.Liability));

    // Equity
    public static readonly Account OwnerEquity = FakeAccounts.Get(x => x.WithName("Owner Equity", AccountType.Equity));

    // Revenue
    public static readonly Account Revenue = FakeAccounts.Get(x => x.WithName("Revenue", AccountType.Revenue));

    // Expenses
    public static readonly Account
        CostOfSales = FakeAccounts.Get(x => x.WithName("Cost of Sales", AccountType.Expense));

    public static readonly Account LaundryCosts =
        FakeAccounts.Get(x => x.WithName("Laundry Costs", AccountType.Expense));
}