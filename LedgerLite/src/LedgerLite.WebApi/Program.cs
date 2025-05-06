using FastEndpoints;
using LedgerLite.Users;
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

    builder.Services
        .AddLedgerLiteAuth()
        .AddModules(builder.Configuration)
        .AddApiInfrastructure(builder.Configuration);

    var app = builder.Build();
    
    app.UseSerilogRequestLogging();
    app.MapOpenApi();
    app.UseAuthorization();
    app.UseFastEndpoints();
    app.MapUserEndpoints();
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