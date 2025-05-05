using FastEndpoints;
using LedgerLite.WebApi;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();
try
{
    Log.Information("Starting LedgerLite web API...");
    
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilog((services, config) => config
        .ReadFrom.Configuration(builder.Configuration));

    builder.Services
        .AddModules(builder.Configuration)
        .AddApiInfrastructure();

    var app = builder.Build();

    app.UseSerilogRequestLogging();
    app.MapOpenApi();
    app.UseFastEndpoints();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "LedgerLite ended abruptly.");
}
finally
{
    Log.CloseAndFlush();
}