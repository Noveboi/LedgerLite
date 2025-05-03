using Ardalis.SmartEnum;

namespace LedgerLite.Accounting.Core.Domain;

public sealed class Currency(string name, int value) : SmartEnum<Currency>(name, value)
{
    public static readonly Currency Euro = new("EUR", 1);
    public static readonly Currency Dollar = new("USD", 2);
    public static readonly Currency BritishPound = new("GBP", 3);
}