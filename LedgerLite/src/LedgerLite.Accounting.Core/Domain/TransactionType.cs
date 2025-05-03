using Ardalis.SmartEnum;

namespace LedgerLite.Accounting.Core.Domain;

public sealed class TransactionType : SmartEnum<TransactionType>
{
    private TransactionType() : this("", 0) { }
    private TransactionType(string name, int value) : base(name, value) { }

    /// <summary>
    /// Increases the value of liability, equity and revenue.
    /// </summary>
    public static readonly TransactionType Credit = new(nameof(Credit), 1); 
    /// <summary>
    /// Debit increases the value of assets and expenses.
    /// </summary>
    public static readonly TransactionType Debit = new(nameof(Debit), 2);
}