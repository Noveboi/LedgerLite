using Bogus;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Tests.Shared;

namespace LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

public sealed class FakeJournalEntryOptions
{
    internal Guid Id = Guid.NewGuid();
    internal Guid? FiscalPeriodId { get; private set; }
    internal List<JournalEntryLine>? Lines { get; private set; }
    internal JournalEntryType? JournalEntryType { get; private set; }

    public FakeJournalEntryOptions WithFiscalPeriod(Guid fiscalPeriodId)
    {
        FiscalPeriodId = fiscalPeriodId;
        return this;
    }

    public FakeJournalEntryOptions AddLine(JournalEntryLine line)
    {
        Lines ??= [];
        Lines.Add(line);
        return this;
    }
    
    public FakeJournalEntryOptions AddLine(Action<FakeJournalEntryLineOptions> configureLine)
    {
        Lines ??= [];
        var options = new FakeJournalEntryLineOptions
        {
            EntryId = Id
        };
        
        configureLine(options);
        var line = FakeJournalEntryLines.GetFakerCore(options).Generate();
        
        Lines.Add(line);
        return this;
    }

    public FakeJournalEntryOptions WithType(JournalEntryType journalEntryType)
    {
        JournalEntryType = journalEntryType;
        return this;
    }
}

public static class FakeJournalEntries
{
    private static readonly Guid FiscalPeriodId = Guid.NewGuid();
    private static Faker<JournalEntry> GetFaker(FakeJournalEntryOptions options) =>
        new PrivateFaker<JournalEntry>(new PrivateBinder())
            .UsePrivateConstructor()
            .RuleFor(x => x.Id, options.Id)
            .RuleFor(x => x.FiscalPeriodId, options.FiscalPeriodId ?? FiscalPeriodId)
            .RuleFor(x => x.ReferenceNumber, f => f.Random.String2(5))
            .RuleFor(x => x.OccursAt, f => DateOnly.FromDateTime(f.Date.Past()))
            .RuleFor(x => x.Type, options.JournalEntryType ?? JournalEntryType.Standard)
            .RuleFor(x => x.Status, JournalEntryStatus.Editable)
            .RuleFor("_lines", _ => options.Lines ?? []);

    public static JournalEntry Get(Action<FakeJournalEntryOptions>? configure = null)
    {
        var options = new FakeJournalEntryOptions();
        configure?.Invoke(options);
        return GetFaker(options).Generate();
    }
}
