using Ardalis.GuardClauses;

namespace LedgerLite.Accounting.Domain.Extensions;
public static class GuardClauseExtensions
{
    public static Money NegativeOrZero(this IGuardClause clause, Money money)
    {
        clause.NegativeOrZero(money.Amount);
        return money;
    }
}