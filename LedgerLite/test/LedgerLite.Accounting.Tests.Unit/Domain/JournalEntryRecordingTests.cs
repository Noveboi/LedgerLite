using Ardalis.Result;
using LedgerLite.Accounting.Domain;
using LedgerLite.Accounting.Domain.JournalEntries;
using LedgerLite.Accounting.Tests.Unit.Fakes;

namespace LedgerLite.Accounting.Tests.Unit.Domain;

public class JournalEntryRecordingTests
{
    [Theory]
    [MemberData(nameof(JournalEntryTypes.AllTypesExceptCompound), MemberType = typeof(JournalEntryTypes))]
    public void Invalid_MoreThanTwoLines_ForNonCompoundEntry(JournalEntryType type)
    {
        var lines = FakeJournalEntryLines.Get(TransactionType.Credit, TransactionType.Credit, TransactionType.Debit);
        var result = JournalEntryHelper.Create(type, lines);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(JournalEntryErrors.MoreThanTwoLinesWhenTypeIsNotCompound(3));
    }

    [Theory]
    [MemberData(nameof(JournalEntryTypes.AllTypes), MemberType = typeof(JournalEntryTypes))]
    public void Invalid_OneLine_ForAllEntryTypes(JournalEntryType type)
    {
        var lines = FakeJournalEntryLines.Get(TransactionType.Credit);
        var result = JournalEntryHelper.Create(type, lines);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(JournalEntryErrors.LessThanTwoLines(1));
    }
    
    [Theory]
    [MemberData(nameof(JournalEntryTypes.AllTypes), MemberType = typeof(JournalEntryTypes))]
    public void Invalid_NoLines_ForAllEntryTypes(JournalEntryType type)
    {
        var result = JournalEntryHelper.Create(type, []);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(JournalEntryErrors.LessThanTwoLines(0));
    }

    [Theory]
    [MemberData(nameof(JournalEntryTypes.AllTypes), MemberType = typeof(JournalEntryTypes))]
    public void Valid_OneCreditLine_And_OneDebitLine(JournalEntryType type)
    {
        var lines = FakeJournalEntryLines.GenerateStandardLines();
        var result = JournalEntryHelper.Create(type, lines);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.Lines.ShouldBeEquivalentTo(lines);
    } 
}