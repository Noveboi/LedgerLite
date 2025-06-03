using FastEndpoints;
using LedgerLite.SharedKernel.Identity;

namespace LedgerLite.Accounting.Reporting.Income.Endpoints;

public sealed record IncomeStatementQuery(DateOnly From, DateOnly To);

public sealed record IncomeStatementRequest(
    [property: FromClaim(LedgerClaims.UserId)] Guid UserId,
    [property: RouteParam] Guid PeriodId,
    [property: FromQuery] IncomeStatementQuery Query);