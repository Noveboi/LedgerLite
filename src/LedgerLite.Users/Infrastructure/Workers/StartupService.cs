using LedgerLite.Users.Application.Roles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace LedgerLite.Users.Infrastructure.Workers;

public sealed class StartupService(IServiceProvider provider) : BackgroundService
{
    private readonly ILogger _log = Log.ForContext<StartupService>();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _log.Information("Running {service}", nameof(StartupService));
        try
        {
            var scope = provider.CreateScope();
            var roleMaker = scope.ServiceProvider.GetRequiredService<RoleMaker>();

            var result = await roleMaker.CreateApplicationRolesAsync(stoppingToken);
            if (!result.IsSuccess) _log.Error("Role maker failed. {@result}", result);
        }
        catch (Exception ex)
        {
            _log.Fatal(ex, "An exception occured in {service}", nameof(StartupService));
            throw;
        }
    }
}