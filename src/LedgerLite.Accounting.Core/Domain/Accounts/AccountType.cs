using Ardalis.SmartEnum;

namespace LedgerLite.Accounting.Core.Domain.Accounts;

public sealed class AccountType : SmartEnum<AccountType>
{
    public static readonly AccountType Asset = new(name: nameof(Asset), value: 1);
    public static readonly AccountType Liability = new(name: nameof(Liability), value: 2);
    public static readonly AccountType Expense = new(name: nameof(Expense), value: 3);
    public static readonly AccountType Revenue = new(name: nameof(Revenue), value: 4);
    public static readonly AccountType Equity = new(name: nameof(Equity), value: 5);

    private AccountType() : this(name: "", value: 0)
    {
    }

    private AccountType(string name, int value) : base(name: name, value: value)
    {
    }

    public bool IsCredit()
    {
        return this == Liability || this == Equity || this == Revenue;
    }

    public bool IsDebit()
    {
        return !IsCredit();
    }
}