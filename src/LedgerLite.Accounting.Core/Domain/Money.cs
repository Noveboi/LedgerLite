namespace LedgerLite.Accounting.Core.Domain;

/// <summary>
///     An amount tied to a specific currency
/// </summary>
public sealed record Money(decimal Amount, Currency Currency)
{
    public static Money operator +(Money a, Money b)
    {
        return ValidateAndOperate(a: a, b: b, (x, y) => x + y);
    }

    public static Money operator -(Money a, Money b)
    {
        return ValidateAndOperate(a: a, b: b, (x, y) => x - y);
    }

    public static Money operator *(Money a, Money b)
    {
        return ValidateAndOperate(a: a, b: b, (x, y) => x * y);
    }

    public static Money operator /(Money a, Money b)
    {
        return ValidateAndOperate(a: a, b: b, (x, y) => x / y);
    }

    private static Money ValidateAndOperate(Money a, Money b, Func<decimal, decimal, decimal> operation)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException(
                message: "Arithmetic operations on money require the same currency be used.");

        return a with { Amount = operation(arg1: a.Amount, arg2: b.Amount) };
    }
}