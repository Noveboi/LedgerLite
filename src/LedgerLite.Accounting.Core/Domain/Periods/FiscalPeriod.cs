using System.ComponentModel.DataAnnotations.Schema;
using Ardalis.Result;
using LedgerLite.SharedKernel.Domain;
using LedgerLite.SharedKernel.Models;

namespace LedgerLite.Accounting.Core.Domain.Periods;

public sealed class FiscalPeriod : AuditableEntity
{
    private FiscalPeriod()
    {
    }

    private FiscalPeriod(Guid organizationId, DateOnly startDate, DateOnly endDate, string name)
    {
        OrganizationId = organizationId;
        StartDate = startDate;
        EndDate = endDate;
        Name = name;
    }

    /// <summary>
    ///     The ID of the organization that created and is using this fiscal period.
    /// </summary>
    public Guid OrganizationId { get; private init; }

    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public DateTime? ClosedAtUtc { get; private set; }
    public string Name { get; private set; } = null!;

    public bool IsClosed => ClosedAtUtc.HasValue;
    [NotMapped] public DateRange Range => new(Start: StartDate, End: EndDate);

    public static Result<FiscalPeriod> Create(Guid organizationId, DateOnly startDate, DateOnly endDate, string name)
    {
        if (startDate > endDate)
            return Result.Invalid(validationError: FiscalPeriodErrors.StartIsAfterEnd(start: startDate, end: endDate));

        if (string.IsNullOrWhiteSpace(value: name))
            return Result.Invalid(validationError: FiscalPeriodErrors.NameCannotBeEmpty());

        var period = new FiscalPeriod(
            organizationId: organizationId,
            startDate: startDate,
            endDate: endDate,
            name: name);

        return period;
    }

    public Result Close()
    {
        throw new NotImplementedException();
    }
}