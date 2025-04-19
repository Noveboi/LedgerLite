namespace LedgerLite.Accounting.Domain;

public enum TransactionType
{
    /// <summary>
    /// Money flowing out, credit increases the value of liability, equity and revenue.
    /// </summary>
    Credit = 1, 
    /// <summary>
    /// Money flowing in, debit increases the value of assets and expenses.
    /// </summary>
    Debit = 2
}