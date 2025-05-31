using Ardalis.SmartEnum;

namespace LedgerLite.Accounting.Core.Domain;

public sealed class Currency : SmartEnum<Currency>
{
    public static readonly Currency Euro = new("EUR", 1);
    public static readonly Currency Dollar = new("USD", 2);
    public static readonly Currency BritishPound = new("GBP", 3);

    private Currency() : this("", 0)
    {
    }

    private Currency(string name, int value) : base(name, value)
    {
    }
}