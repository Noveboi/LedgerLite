using LedgerLite.Accounting.Domain;

namespace LedgerLite.Accounting.Tests.Unit.Domain;

public class MoneyArithmeticTests
{
    private static (Money, Money) Get(decimal a, decimal b) => (new Money(a, Currency.Euro), new Money(b, Currency.Euro));
    
    [Fact]
    public void Addition()
    {
        var (a, b) = Get(10, 5);
        var result = a + b;
        result.Amount.ShouldBe(15);
    }

    [Fact]
    public void Subtraction()
    {
        var (a, b) = Get(10, 5);
        var result = a - b;
        result.Amount.ShouldBe(5);
    }

    [Fact]
    public void Multiplication()
    {
        var (m1, m2) = Get(4, 9);
        var result = m1 * m2;
        result.Amount.ShouldBe(36);
    }

    [Fact]
    public void Division()
    {
        var (a, b) = Get(12, 3);
        var result = a / b;
        result.Amount.ShouldBe(4);
    }

    [Fact] public void Add_ThrowOn_DifferentCurrencies() => AssertDifferentCurrencies((a, b) => a + b);
    [Fact] public void Subtract_ThrowOn_DifferentCurrencies() => AssertDifferentCurrencies((a, b) => a - b);
    [Fact] public void Multiply_ThrowOn_DifferentCurrencies() => AssertDifferentCurrencies((a, b) => a * b);
    [Fact] public void Division_ThrowOn_DifferentCurrencies() => AssertDifferentCurrencies((a, b) => a / b);
    
    private void AssertDifferentCurrencies(Func<Money, Money, Money> operation)
    {
        var a = new Money(1337, Currency.Euro);
        var b = new Money(420, Currency.Dollar);
        
        var action = () => operation(a, b);
        action.ShouldThrow<InvalidOperationException>();
    } 
}