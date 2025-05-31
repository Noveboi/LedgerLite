using LedgerLite.Accounting.Core.Domain.JournalEntries;

namespace LedgerLite.Accounting.Tests.Unit.Utilities;

public static class JournalEntryTypes
{
    public static TheoryData<JournalEntryType> AllTypesExceptCompound => new(
        values: JournalEntryType.List.Except(second: [JournalEntryType.Compound]));

    public static TheoryData<JournalEntryType> AllTypes => new(values: JournalEntryType.List);
}