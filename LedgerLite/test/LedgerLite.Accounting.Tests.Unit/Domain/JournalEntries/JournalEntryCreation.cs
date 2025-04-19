using LedgerLite.Accounting.Domain;
using Shouldly;

namespace LedgerLite.Accounting.Tests.Unit.Domain.JournalEntries;

public class JournalEntryCreation
{
    private static Guid Id => Guid.NewGuid();
    
    [Fact]
    public void CreateDebitEntry_ReturnsJournalEntry_OfTypeDebit()
    {
        var entry = JournalEntry.Debit(1, Id);
        entry.Type.ShouldBe(JournalEntryType.Debit);
    }

    [Fact]
    public void CreateCreditEntry_ReturnsJournalEntry_OfTypeCredit()
    {
        var entry = JournalEntry.Credit(1, Id);
        entry.Type.ShouldBe(JournalEntryType.Credit);
    }

    [Theory]
    [InlineData("Debit")]
    [InlineData("Credit")]
    public void NegativeMoney_ThrowsException(string type)
    {
        const int money = -1;
        var createAction = () => type == "Debit" ? JournalEntry.Debit(money, Id) : JournalEntry.Credit(money, Id);
        createAction.ShouldThrow<ArgumentException>();
    }
}