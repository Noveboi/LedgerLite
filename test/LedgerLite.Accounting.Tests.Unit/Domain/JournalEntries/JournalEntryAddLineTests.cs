using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Tests.Unit.Utilities;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

namespace LedgerLite.Accounting.Tests.Unit.Domain.JournalEntries;

public class JournalEntryAddLineTests
{
    private static readonly Guid Id = Guid.NewGuid();

    [Fact]
    public void Invalid_WhenNotEditable()
    {
        var lines = FakeJournalEntryLines.GenerateStandardLines();
        var entry = JournalEntryHelper.CreateWithLines(JournalEntryType.Standard, lines);
        entry.Post();

        var result = entry.AddLine(Id, TransactionType.Credit, 10);

        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(JournalEntryErrors.CannotEdit(entry.Status));
    }

    [Fact]
    public void AddsToList_WhenValid()
    {
        var entry = JournalEntryHelper.Create(JournalEntryType.Standard);
        var expected = JournalEntryLine.Create(TransactionType.Credit, 10, Id, entry.Id);

        var result = entry.AddLine(Id, TransactionType.Credit, 10);

        result.Status.ShouldBe(ResultStatus.Ok);
        var line = entry.Lines.ShouldHaveSingleItem();
        line.Amount.ShouldBe(expected.Amount);
    }
}