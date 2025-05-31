using LedgerLite.Accounting.Core.Domain;

namespace LedgerLite.Accounting.Tests.Unit.Domain;

public class MoneyArithmeticTests
{
    private static (Money, Money) Get(decimal a, decimal b)
    {
        return (new Money(Amount: a, Currency: Currency.Euro), new Money(Amount: b, Currency: Currency.Euro));
    }

    [Fact]
    public void Addition()
    {
        var (a, b) = Get(a: 10, b: 5);
        var result = a + b;
        result.Amount.ShouldBe(expected: 15);
    }

    [Fact]
    public void Subtraction()
    {
        var (a, b) = Get(a: 10, b: 5);
        var result = a - b;
        result.Amount.ShouldBe(expected: 5);
    }

    [Fact]
    public void Multiplication()
    {
        var (m1, m2) = Get(a: 4, b: 9);
        var result = m1 * m2;
        result.Amount.ShouldBe(expected: 36);
    }

    [Fact]
    public void Division()
    {
        var (a, b) = Get(a: 12, b: 3);
        var result = a / b;
        result.Amount.ShouldBe(expected: 4);
    }

    [Fact]
    public void Add_ThrowOn_DifferentCurrencies()
    {
        AssertDifferentCurrencies(operation: (a, b) => a + b);
    }

    [Fact]
    public void Subtract_ThrowOn_DifferentCurrencies()
    {
        AssertDifferentCurrencies(operation: (a, b) => a - b);
    }

    [Fact]
    public void Multiply_ThrowOn_DifferentCurrencies()
    {
        AssertDifferentCurrencies(operation: (a, b) => a * b);
    }

    [Fact]
    public void Division_ThrowOn_DifferentCurrencies()
    {
        AssertDifferentCurrencies(operation: (a, b) => a / b);
    }

    private void AssertDifferentCurrencies(Func<Money, Money, Money> operation)
    {
        var a = new Money(Amount: 1337, Currency: Currency.Euro);
        var b = new Money(Amount: 420, Currency: Currency.Dollar);

        var action = () => operation(arg1: a, arg2: b);
        action.ShouldThrow<InvalidOperationException>();
    }
}