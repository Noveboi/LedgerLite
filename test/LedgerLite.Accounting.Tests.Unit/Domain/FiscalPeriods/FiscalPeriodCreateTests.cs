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
        var start = new DateOnly(year: 2025, month: 1, day: 1);
        var end = new DateOnly(year: 2024, month: 1, day: 1);

        var result = FiscalPeriod.Create(organizationId: OrganizationId, startDate: start, endDate: end, name: "Test!");

        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ShouldHaveError(FiscalPeriodErrors.StartIsAfterEnd(start: start, end: end));
    }
}