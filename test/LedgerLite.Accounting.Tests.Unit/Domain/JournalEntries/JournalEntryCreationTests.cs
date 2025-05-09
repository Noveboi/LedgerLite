using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.Accounting.Tests.Unit.Utilities;
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
    
    private static Result<JournalEntry> HelpCreateEntry(string referenceNumber, FiscalPeriod period) => JournalEntry.Create(
        referenceNumber: referenceNumber,
        fiscalPeriod: period,
        type: JournalEntryType.Standard, 
        description: "",
        occursAtUtc: DateTime.UtcNow,
        createdByUserId: Guid.NewGuid());
}