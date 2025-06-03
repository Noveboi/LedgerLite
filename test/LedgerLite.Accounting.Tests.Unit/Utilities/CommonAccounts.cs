using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

namespace LedgerLite.Accounting.Tests.Unit.Utilities;

public static class CommonAccounts
{
    // Assets
    public static readonly Account Cash =
        FakeAccounts.Get(x => x.WithName(name: "Cash", type: AccountType.Asset));

    public static readonly Account Supplies =
        FakeAccounts.Get(x => x.WithName(name: "Supplies", type: AccountType.Asset));

    public static readonly Account Equipment =
        FakeAccounts.Get(x => x.WithName(name: "Equipment", type: AccountType.Asset));

    // Liabilities
    public static readonly Account AccountsPayable =
        FakeAccounts.Get(x => x.WithName(name: "Accounts Payable", type: AccountType.Liability));

    public static readonly Account LoansPayable =
        FakeAccounts.Get(x => x.WithName(name: "Loans Payable", type: AccountType.Liability));

    // Equity
    public static readonly Account OwnerEquity =
        FakeAccounts.Get(x => x.WithName(name: "Owner Equity", type: AccountType.Equity));

    // Revenue
    public static readonly Account Revenue =
        FakeAccounts.Get(x => x.WithName(name: "Revenue", type: AccountType.Revenue));

    // Expenses
    public static readonly Account
        CostOfSales = FakeAccounts.Get(x => x.WithName(name: "Cost of Sales", type: AccountType.Expense));

    public static readonly Account LaundryCosts =
        FakeAccounts.Get(x => x.WithName(name: "Laundry Costs", type: AccountType.Expense));
}