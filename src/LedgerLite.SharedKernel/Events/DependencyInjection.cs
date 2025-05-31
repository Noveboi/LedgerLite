using LedgerLite.SharedKernel.Internal;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LedgerLite.SharedKernel.Events;

internal static class DependencyInjection
{
    public static IServiceCollection AddEventInfrastructure(this IServiceCollection services)
    {
        Log.Information(messageTemplate: "Configuring {name}", propertyValue: "Event Pub/Sub Infrastructure");
        AssemblyScanner.RegisterServices(services: services,
            configure: o => o.Implementing(type: typeof(IEventHandler<>)));
        return services
            .AddScoped<IPublisher, Publisher>()
            .AddScoped<IEventPublisher, SequentialEventPublisher>();
    }
}