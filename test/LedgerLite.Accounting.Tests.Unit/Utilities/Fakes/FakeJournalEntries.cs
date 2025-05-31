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
        Lines.Add(item: line);
        return this;
    }

    public FakeJournalEntryOptions AddLine(Action<FakeJournalEntryLineOptions> configureLine)
    {
        Lines ??= [];
        var options = new FakeJournalEntryLineOptions
        {
            EntryId = Id
        };

        configureLine(obj: options);
        var line = FakeJournalEntryLines.GetFakerCore(options: options).Generate();

        Lines.Add(item: line);
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

    private static Faker<JournalEntry> GetFaker(FakeJournalEntryOptions options)
    {
        return new PrivateFaker<JournalEntry>(binder: new PrivateBinder())
            .UsePrivateConstructor()
            .RuleFor(property: x => x.Id, value: options.Id)
            .RuleFor(property: x => x.FiscalPeriodId, value: options.FiscalPeriodId ?? FiscalPeriodId)
            .RuleFor(property: x => x.ReferenceNumber, setter: f => f.Random.String2(length: 5))
            .RuleFor(property: x => x.OccursAt, setter: f => DateOnly.FromDateTime(dateTime: f.Date.Past()))
            .RuleFor(property: x => x.Type, value: options.JournalEntryType ?? JournalEntryType.Standard)
            .RuleFor(property: x => x.Status, value: JournalEntryStatus.Editable)
            .RuleFor(propertyOrFieldName: "_lines", setter: _ => options.Lines ?? []);
    }

    public static JournalEntry Get(Action<FakeJournalEntryOptions>? configure = null)
    {
        var options = new FakeJournalEntryOptions();
        configure?.Invoke(obj: options);
        return GetFaker(options: options).Generate();
    }
}