using Ardalis.Result;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.Accounting.Core.Domain.Periods;

public sealed class FiscalPeriod : AuditableEntity
{
    private FiscalPeriod() { }
    private FiscalPeriod(Guid organizationId, DateOnly startDate, DateOnly endDate)
    {
        OrganizationId = organizationId;
        StartDate = startDate;
        EndDate = endDate;
    }

    /// <summary>
    /// The ID of the organization that created and is using this fiscal period.
    /// </summary>
    public Guid OrganizationId { get; private init; }
    
    public DateOnly StartDate { get; private init; }
    public DateOnly EndDate { get; private init; }
    
    public DateTime? ClosedAtUtc { get; private set; }

    public bool IsClosed => ClosedAtUtc.HasValue;

    public Result<FiscalPeriod> Create(Guid organizationId, DateOnly startDate, DateOnly endDate)
    {
        if (startDate > endDate)
            return Result.Invalid(FiscalPeriodErrors.StartIsAfterEnd(startDate, endDate));

        var period = new FiscalPeriod(
            organizationId, 
            startDate: startDate, 
            endDate: endDate);

        return period;
    }

    public Result Close()
    {
        throw new NotImplementedException();
    }
}