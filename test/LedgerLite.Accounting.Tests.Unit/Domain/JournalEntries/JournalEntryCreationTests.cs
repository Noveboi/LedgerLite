using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;
using LedgerLite.Tests.Shared;

namespace LedgerLite.Accounting.Tests.Unit.Domain.JournalEntries;

public class JournalEntryCreationTests
{
    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public void Invalid_WhenReferenceNumberIsEmptyOrWhitespace(string refNumber)
    {
        var result = HelpCreateEntry(refNumber, FakeFiscalPeriods.Get());
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ShouldHaveError(JournalEntryErrors.EmptyReferenceNumber());
    }

    [Fact]
    public void Invalid_WhenFiscalPeriod_IsClosed()
    {
        var period = FakeFiscalPeriods.GetClosed();
        var result = HelpCreateEntry("abc123", period);
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ShouldHaveError(JournalEntryErrors.CannotEditBecausePeriodIsClosed(period));
    }
    
    private static Result<JournalEntry> HelpCreateEntry(string referenceNumber, FiscalPeriod period) => JournalEntry.Create(
        referenceNumber: referenceNumber,
        fiscalPeriod: period,
        type: JournalEntryType.Standard, 
        description: "",
        occursAt: DateOnly.FromDateTime(DateTime.UtcNow),
        createdByUserId: Guid.NewGuid());
}