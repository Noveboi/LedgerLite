using Ardalis.Result;
using LedgerLite.Accounting.Domain;
using LedgerLite.Accounting.Domain.JournalEntries;
using LedgerLite.Accounting.Tests.Unit.Utilities;

namespace LedgerLite.Accounting.Tests.Unit.Domain.JournalEntries;

public class JournalEntryPostValidationTests
{
    [Theory]
    [MemberData(nameof(JournalEntryTypes.AllTypesExceptCompound), MemberType = typeof(JournalEntryTypes))]
    public void Invalid_MoreThanTwoLines_ForNonCompoundEntry(JournalEntryType type)
    {
        var lines = FakeJournalEntryLines.Get(TransactionType.Credit, TransactionType.Credit, TransactionType.Debit);
        var entry = JournalEntryHelper.CreateWithLines(type, lines);

        var result = entry.Post();
        
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
        var entry = JournalEntryHelper.CreateWithLines(type, lines);
        
        var result = entry.Post();
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(JournalEntryErrors.LessThanTwoLines(1));
    }
    
    [Theory]
    [MemberData(nameof(JournalEntryTypes.AllTypes), MemberType = typeof(JournalEntryTypes))]
    public void Invalid_NoLines_ForAllEntryTypes(JournalEntryType type)
    {
        var entry = JournalEntryHelper.CreateWithLines(type, []);
           
        var result = entry.Post();

        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(JournalEntryErrors.LessThanTwoLines(0));
    }

    [Theory]
    [MemberData(nameof(JournalEntryTypes.AllTypes), MemberType = typeof(JournalEntryTypes))]
    public void Invalid_TwoCreditLines(JournalEntryType type)
    {
        var lines = FakeJournalEntryLines.Get(TransactionType.Credit, TransactionType.Credit);
        var entry = JournalEntryHelper.CreateWithLines(type, lines);
           
        var result = entry.Post();

        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(JournalEntryErrors.SameTransactionTypeOnBothLines(TransactionType.Credit));
    }
    
    [Theory]
    [MemberData(nameof(JournalEntryTypes.AllTypes), MemberType = typeof(JournalEntryTypes))]
    public void Invalid_TwoDebitLines(JournalEntryType type)
    {
        var lines = FakeJournalEntryLines.Get(TransactionType.Debit, TransactionType.Debit);
        var entry = JournalEntryHelper.CreateWithLines(type, lines);
           
        var result = entry.Post();

        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(JournalEntryErrors.SameTransactionTypeOnBothLines(TransactionType.Debit));
    }

    [Fact]
    public void Invalid_WhenEntryIsAlreadyPosted()
    {
        var lines = FakeJournalEntryLines.GenerateStandardLines();
        var entry = JournalEntryHelper.CreateWithLines(JournalEntryType.Standard, lines);
        entry.Post();
        
        var result = entry.Post();
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(JournalEntryErrors.AlreadyPosted());
    }
    
    [Fact]
    public void Invalid_WhenEntryIsReversed()
    {
        var lines = FakeJournalEntryLines.GenerateStandardLines();
        var entry = JournalEntryHelper.CreateWithLines(JournalEntryType.Standard, lines);
        entry.Reverse();

        var result = entry.Post();
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(JournalEntryErrors.CantPostBecauseIsReversed());
    }
    
    [Theory]
    [MemberData(nameof(JournalEntryTypes.AllTypes), MemberType = typeof(JournalEntryTypes))]
    public void Valid_OneCreditLine_And_OneDebitLine(JournalEntryType type)
    {
        var lines = FakeJournalEntryLines.GenerateStandardLines();
        var entry = JournalEntryHelper.CreateWithLines(type, lines);
           
        var result = entry.Post();

        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Valid_ManyLines_WhenCompound()
    {
        var credit = FakeJournalEntryLines.GetCreditFaker(o => o.Amount = 5).Generate(3);
        var debits = FakeJournalEntryLines.GetDebitFaker(o => o.Amount = 3).Generate(5);
        var entry = JournalEntryHelper.CreateWithLines(JournalEntryType.Compound, [..credit, ..debits]);

        var result = entry.Post();
        
        result.Status.ShouldBe(ResultStatus.Ok);
    }
}