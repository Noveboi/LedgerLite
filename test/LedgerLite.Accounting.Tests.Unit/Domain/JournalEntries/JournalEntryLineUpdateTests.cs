using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Tests.Unit.Utilities;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

namespace LedgerLite.Accounting.Tests.Unit.Domain.JournalEntries;

public class JournalEntryLineUpdateTests
{
    private readonly JournalEntryLine _cashLine;
    private readonly JournalEntryLine _costLine;
    private readonly JournalEntry _entry;

    public JournalEntryLineUpdateTests()
    {
        _entry = FakeJournalEntries.Get(x => x
            .AddLine(y => y.Credit(CommonAccounts.Cash, 100))
            .AddLine(y => y.Debit(CommonAccounts.CostOfSales, 100))
            .WithDescription("Hello there!"));
        
        _cashLine = _entry.Lines.First(x => x.Account == CommonAccounts.Cash);
        _costLine = _entry.Lines.First(x => x.Account == CommonAccounts.CostOfSales);
    }

    [Fact]
    public void Update_LastModifiedByUser()
    {
        var id = Guid.NewGuid();
        
        var result = _entry.Update(id, null, null, null);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        _entry.LastModifiedByUserId.ShouldBe(id);
    }

    [Fact]
    public void Update_Description()
    {
        _entry.Update(Guid.Empty, "New stuff!", null, null);
        _entry.Description.ShouldBe("New stuff!");
    }

    [Fact]
    public void Update_OccursAt()
    {
        var date = new DateOnly(1966, 9, 9);
        _entry.Update(Guid.Empty, null, date, null);
        _entry.OccursAt.ShouldBe(date);
    }

    [Fact]
    public void Update_LineAmount_AlsoUpdatesAmountOfOtherLine()
    {
        var request = new UpdateLineRequest(_cashLine.Id, null, 200, null);
        _entry.Update(Guid.Empty, null, null, request);
        _cashLine.Amount.ShouldBe(200);
        _costLine.Amount.ShouldBe(200);
    }

    [Fact]
    public void Update_TransactionType_AlsoSwapsTypesOfOtherLine()
    {
        var request = new UpdateLineRequest(_cashLine.Id, TransactionType.Debit, null, null);
        _entry.Update(Guid.Empty, null, null, request);
        _cashLine.TransactionType.ShouldBe(TransactionType.Debit);
        _costLine.TransactionType.ShouldBe(TransactionType.Credit);
    }

    [Fact]
    public void Update_TransferAccountId()
    {
        var id = Guid.NewGuid();
        var request = new UpdateLineRequest(_cashLine.Id, null, null, id);
        _entry.Update(Guid.Empty, null, null, request);
        _costLine.AccountId.ShouldBe(id);
    }
}