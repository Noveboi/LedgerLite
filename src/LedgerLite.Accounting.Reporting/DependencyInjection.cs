using LedgerLite.Accounting.Reporting.Trial;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerLite.Accounting.Reporting;

public static class DependencyInjection
{
    public static IServiceCollection AddReportingModule(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddScoped<ReportingUserAuthorization>()
            .AddScoped<TrialBalanceService>();
    }
}