namespace LedgerLite.Accounting.Core.Domain;

/// <summary>
///     An amount tied to a specific currency
/// </summary>
public sealed record Money(decimal Amount, Currency Currency)
{
    public static Money operator +(Money a, Money b)
    {
        return ValidateAndOperate(a, b, (x, y) => x + y);
    }

    public static Money operator -(Money a, Money b)
    {
        return ValidateAndOperate(a, b, (x, y) => x - y);
    }

    public static Money operator *(Money a, Money b)
    {
        return ValidateAndOperate(a, b, (x, y) => x * y);
    }

    public static Money operator /(Money a, Money b)
    {
        return ValidateAndOperate(a, b, (x, y) => x / y);
    }

    private static Money ValidateAndOperate(Money a, Money b, Func<decimal, decimal, decimal> operation)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Arithmetic operations on money require the same currency be used.");

        return a with { Amount = operation(a.Amount, b.Amount) };
    }
}