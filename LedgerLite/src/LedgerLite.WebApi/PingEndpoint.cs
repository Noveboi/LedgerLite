using FastEndpoints;

namespace LedgerLite.WebApi;

public sealed record PingResponse(string Message, string Status);
public sealed class PingEndpoint : EndpointWithoutRequest<PingResponse>
{
    public override void Configure()
    {
        Get("/");
        AllowAnonymous();
        
        Summary(endpoint =>
        {
            endpoint.Summary = "Used to ping the LedgerLite API for testing or availability";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(
            response: new PingResponse("Hello!", "Healthy"), 
            cancellation: ct);
    }
}