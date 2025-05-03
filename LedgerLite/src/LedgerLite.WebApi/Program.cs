using FastEndpoints;
using LedgerLite.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddModules(builder.Configuration)
    .AddApiInfrastructure();

var app = builder.Build();

app.MapOpenApi();
app.UseFastEndpoints();
app.Run();