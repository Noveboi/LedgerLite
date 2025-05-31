using FastEndpoints;
using LedgerLite.SharedKernel;
using LedgerLite.Users;
using LedgerLite.WebApi;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override(source: "Microsoft", minimumLevel: LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information(messageTemplate: "Starting LedgerLite web API...");
    var builder = WebApplication.CreateBuilder(args: args);

    builder.Services
        .AddLedgerLiteCors(configuration: builder.Configuration)
        .AddLedgerLiteAuth()
        .AddModules(configuration: builder.Configuration)
        .AddApiInfrastructure(configuration: builder.Configuration)
        .AddSharedKernelServices();

    var app = builder.Build();

    if (app.Environment.IsProduction()) app.UseHttpsRedirection();

    app.UseSerilogRequestLogging();
    app.UseCors(policyName: "LedgerLite");
    app.MapOpenApi();
    app.UseAuthorization();
    app.UseFastEndpoints();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(exception: ex, messageTemplate: "LedgerLite ended abruptly.");
}
finally
{
    Log.CloseAndFlush();
}