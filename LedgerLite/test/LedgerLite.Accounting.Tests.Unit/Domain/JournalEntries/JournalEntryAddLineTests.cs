using Ardalis.Result;
using LedgerLite.Accounting.Domain;
using LedgerLite.Accounting.Domain.JournalEntries;
using LedgerLite.Accounting.Tests.Unit.Utilities;

namespace LedgerLite.Accounting.Tests.Unit.Domain.JournalEntries;

public class JournalEntryAddLineTests
{
    private static readonly Guid _id = Guid.NewGuid();
    
    [Fact]
    public void Invalid_WhenAmountIsNotPositive()
    {
        var entry = JournalEntryHelper.Create(JournalEntryType.Standard);
        
        var result = entry.AddLine(_id, TransactionType.Credit, -10);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(JournalEntryErrors.NonPositiveAmount(-10));
    }
    
    [Fact]
    public void Invalid_WhenAmountIsZero()
    {
        var entry = JournalEntryHelper.Create(JournalEntryType.Standard);

        var result = entry.AddLine(_id, TransactionType.Credit, 0);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(JournalEntryErrors.NonPositiveAmount(0));
    }

    [Fact]
    public void Invalid_WhenNotEditable()
    {
        var lines = FakeJournalEntryLines.GenerateStandardLines();
        var entry = JournalEntryHelper.CreateWithLines(JournalEntryType.Standard, lines);
        entry.Post();

        var result = entry.AddLine(_id, TransactionType.Credit, 10);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(JournalEntryErrors.CannotEdit(entry.Status));
    }

    [Fact]
    public void DoesNotAddToList_WhenInvalid()
    {
        var entry = JournalEntryHelper.Create(JournalEntryType.Standard);

        var result = entry.AddLine(_id, TransactionType.Debit, -12.5m);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        entry.Lines.ShouldBeEmpty();
    }
    
    [Fact]
    public void AddsToList_WhenValid()
    {
        var entry = JournalEntryHelper.Create(JournalEntryType.Standard);
        var expected = JournalEntryLine.Create(TransactionType.Credit, 10, _id, entry.Id);
        
        var result = entry.AddLine(_id, TransactionType.Credit, 10);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        var line = entry.Lines.ShouldHaveSingleItem();
        line.Amount.ShouldBe(expected.Amount);
    }
}