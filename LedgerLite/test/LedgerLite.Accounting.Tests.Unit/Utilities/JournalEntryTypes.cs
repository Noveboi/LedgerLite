using LedgerLite.Accounting.Domain.JournalEntries;

namespace LedgerLite.Accounting.Tests.Unit.Utilities;

public static class JournalEntryTypes
{
    public static TheoryData<JournalEntryType> AllTypesExceptCompound => new(
        JournalEntryType.List.Except([JournalEntryType.Compound]));
    
    public static TheoryData<JournalEntryType> AllTypes => new(JournalEntryType.List);
}