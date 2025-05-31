namespace LedgerLite.Accounting.Core.Domain;

public enum TransactionType
{
    /// <summary>
    ///     Increases the value of liability, equity and revenue.
    /// </summary>
    Credit = 1,

    /// <summary>
    ///     Increases the value of assets and expenses.
    /// </summary>
    Debit = 2
}