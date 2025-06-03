namespace LedgerLite.Accounting.Reporting.Income.Endpoints;

internal sealed record IncomeStatementMetricsDto(decimal GrossProfitMargin);

internal sealed record IncomeStatementDto(
    decimal Revenue,
    decimal DirectExpenses,
    decimal IndirectExpenses,
    decimal Interest,
    decimal Tax,
    decimal GrossProfit,
    decimal OperatingProfit,
    decimal NetProfit,
    IncomeStatementMetricsDto Metrics)
{
    public static IncomeStatementDto FromEntity(IncomeStatement statement) =>
        new(Revenue: statement.TotalRevenue,
            DirectExpenses: statement.DirectExpenses,
            IndirectExpenses: statement.IndirectExpenses,
            Interest: statement.InterestExpense,
            Tax: statement.TaxExpense,
            GrossProfit: statement.GrossProfit,
            OperatingProfit: statement.OperatingProfit,
            NetProfit: statement.NetProfit,
            Metrics: new IncomeStatementMetricsDto(
                GrossProfitMargin: statement.GetGrossProfitMargin()));
}