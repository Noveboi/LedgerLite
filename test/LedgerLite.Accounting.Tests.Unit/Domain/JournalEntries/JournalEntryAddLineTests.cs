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
        var entry = JournalEntryHelper.CreateWithLines(type: JournalEntryType.Standard, lines: lines);
        entry.Post();

        var result = entry.AddLine(accountId: Id, type: TransactionType.Credit, amount: 10);

        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(JournalEntryErrors.CannotEdit(status: entry.Status));
    }

    [Fact]
    public void AddsToList_WhenValid()
    {
        var entry = JournalEntryHelper.Create(type: JournalEntryType.Standard);
        var expected =
            JournalEntryLine.Create(type: TransactionType.Credit, amount: 10, accountId: Id, entryId: entry.Id);

        var result = entry.AddLine(accountId: Id, type: TransactionType.Credit, amount: 10);

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        var line = entry.Lines.ShouldHaveSingleItem();
        line.Amount.ShouldBe(expected: expected.Amount);
    }
}