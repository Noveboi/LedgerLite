using Ardalis.SmartEnum;

namespace LedgerLite.Accounting.Core.Domain;

public sealed class Currency : SmartEnum<Currency>
{
    public static readonly Currency Euro = new(name: "EUR", value: 1);
    public static readonly Currency Dollar = new(name: "USD", value: 2);
    public static readonly Currency BritishPound = new(name: "GBP", value: 3);

    private Currency() : this(name: "", value: 0)
    {
    }

    private Currency(string name, int value) : base(name: name, value: value)
    {
    }
}