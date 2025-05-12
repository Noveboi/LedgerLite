using Bogus;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.Tests.Shared;

namespace LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

public sealed class FakeFiscalPeriodConfiguration
{
    internal DateOnly? StartDate { get; private set; }
    internal DateOnly? EndDate { get; private set; }
    internal Guid? OrganizationId { get; private set; }

    public FakeFiscalPeriodConfiguration StartingAt(DateOnly start)
    {
        StartDate = start;
        return this;
    }

    public FakeFiscalPeriodConfiguration EndingAt(DateOnly end)
    {
        EndDate = end;
        return this;
    }

    public FakeFiscalPeriodConfiguration WithOrganization(Guid id)
    {
        OrganizationId = id;
        return this;
    }
}

public static class FakeFiscalPeriods
{
    private static Faker<FiscalPeriod> Faker(FakeFiscalPeriodConfiguration config) => new PrivateFaker<FiscalPeriod>()
        .UsePrivateConstructor()
        .RuleFor(x => x.StartDate, f => config.StartDate ?? f.Date.PastDateOnly())
        .RuleFor(x => x.EndDate, (_, period) => config.EndDate ?? period.StartDate.AddMonths(4))
        .RuleFor(x => x.OrganizationId, _ => config.OrganizationId ?? Guid.NewGuid());

    public static FiscalPeriod Get(Action<FakeFiscalPeriodConfiguration>? configure = null)
    {
        var config = new FakeFiscalPeriodConfiguration();
        configure?.Invoke(config);
        
        return Faker(config).Generate();
    }

    public static FiscalPeriod GetClosed(Action<FakeFiscalPeriodConfiguration>? configure = null)
    {
        var config = new FakeFiscalPeriodConfiguration();
        configure?.Invoke(config);
        
        return Faker(config)
            .RuleFor(x => x.ClosedAtUtc, f => f.Date.Past())
            .Generate();
    }
}