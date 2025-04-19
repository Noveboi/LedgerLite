using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddFastEndpoints();

var app = builder.Build();

app.MapOpenApi();
app.UseFastEndpoints();
app.Run();