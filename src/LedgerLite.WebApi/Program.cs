using FastEndpoints;
using LedgerLite.SharedKernel;
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
        .AddLedgerLiteCors(builder.Configuration)
        .AddLedgerLiteAuth()
        .AddModules(builder.Configuration)
        .AddApiInfrastructure(builder.Configuration)
        .AddSharedKernelServices();

    var app = builder.Build();

    if (app.Environment.IsProduction())
    {
        app.UseHttpsRedirection();
    }
    
    app.UseSerilogRequestLogging();
    app.UseCors("LedgerLite");
    app.MapOpenApi();
    app.UseAuthorization();
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