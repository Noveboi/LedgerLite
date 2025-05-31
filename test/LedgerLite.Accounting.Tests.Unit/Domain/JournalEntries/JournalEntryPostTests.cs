using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Tests.Unit.Utilities;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

namespace LedgerLite.Accounting.Tests.Unit.Domain.JournalEntries;

public class JournalEntryPostTests
{
    [Fact]
    public void ChangeStatus_ToPosted()
    {
        var lines = FakeJournalEntryLines.GenerateStandardLines();
        var entry = JournalEntryHelper.CreateWithLines(JournalEntryType.Standard, lines);

        var result = entry.Post();

        result.Status.ShouldBe(ResultStatus.Ok);
        entry.Status.ShouldBe(JournalEntryStatus.Posted);
    }
}