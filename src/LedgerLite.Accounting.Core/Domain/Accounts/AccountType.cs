using Ardalis.SmartEnum;

namespace LedgerLite.Accounting.Core.Domain.Accounts;

public sealed class AccountType : SmartEnum<AccountType>
{
    private AccountType() : this("", 0) { }
    private AccountType(string name, int value) : base(name, value) { }

    public static readonly AccountType Asset = new(nameof(Asset), 1);
    public static readonly AccountType Liability = new(nameof(Liability), 2);
    public static readonly AccountType Expense = new(nameof(Expense), 3);
    public static readonly AccountType Revenue = new(nameof(Revenue), 4);
    public static readonly AccountType Equity = new(nameof(Equity), 5);
    
    public bool IsCredit() => this == Liability || this == Equity || this == Revenue;
    public bool IsDebit() => !IsCredit();
}