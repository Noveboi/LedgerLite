using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.Tests.Shared;

namespace LedgerLite.Accounting.Tests.Unit.Domain.FiscalPeriods;

public class FiscalPeriodCreateTests
{
    private static readonly Guid OrganizationId = Guid.NewGuid();

    [Fact]
    public void Invalid_WhenStartDateAfterEndDate()
    {
        var start = new DateOnly(2025, 1, 1);
        var end = new DateOnly(2024, 1, 1);

        var result = FiscalPeriod.Create(OrganizationId, start, end, "Test!");

        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ShouldHaveError(FiscalPeriodErrors.StartIsAfterEnd(start, end));
    }
}