using FastEndpoints;
using LedgerLite.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddModules()
    .AddInfrastructure();

var app = builder.Build();

app.MapOpenApi();
app.UseFastEndpoints();
app.Run();