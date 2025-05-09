using Bogus;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.Tests.Shared;

namespace LedgerLite.Accounting.Tests.Unit.Utilities;

public static class FakeFiscalPeriods
{
    private static Faker<FiscalPeriod> Faker() => new PrivateFaker<FiscalPeriod>()
        .UsePrivateConstructor()
        .RuleFor(x => x.StartDate, f => f.Date.PastDateOnly())
        .RuleFor(x => x.EndDate, (_, period) => period.StartDate.AddMonths(4))
        .RuleFor(x => x.OrganizationId, _ => Guid.NewGuid());

    public static FiscalPeriod Get() => Faker().Generate();

    public static FiscalPeriod GetClosed() => Faker()
        .RuleFor(x => x.ClosedAtUtc, f => f.Date.Past())
        .Generate();
}