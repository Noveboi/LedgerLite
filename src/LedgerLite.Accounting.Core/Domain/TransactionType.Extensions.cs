using Ardalis.Result;

namespace LedgerLite.Accounting.Core.Domain;

public static class TransactionTypes
{
    public static Result<(TransactionType? Type, decimal? Amount)> GetTransactionType(decimal? credit, decimal? debit) =>
        (credit, debit) switch
        {
            (null, null) => Result<(TransactionType?, decimal?)>.Success((null, null)),
            (> 0 and var x, null or 0) => (TransactionType.Credit, x),
            (null or 0, > 0 and var x) => (TransactionType.Debit, x),
            _ => Result.Invalid(new ValidationError("Invalid debits/credits."))
        };
    
    public static TransactionType Opposite(this TransactionType transactionType) => 
        transactionType is TransactionType.Credit
            ? TransactionType.Debit
            : TransactionType.Credit;
}