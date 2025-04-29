using Ardalis.SmartEnum;

namespace LedgerLite.Accounting.Domain.Accounts;

public sealed class AccountType(string name, int value) : SmartEnum<AccountType>(name, value)
{
    public static readonly AccountType Asset = new(nameof(Asset), 1);
    public static readonly AccountType Liability = new(nameof(Liability), 2);
    public static readonly AccountType Expense = new(nameof(Expense), 3);
    public static readonly AccountType Income = new(nameof(Income), 4);
    public static readonly AccountType Equity = new(nameof(Equity), 5);
}