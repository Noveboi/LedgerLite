using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LedgerLite.SharedKernel.Events;

internal static class DependencyInjection
{
    public static IServiceCollection AddEventInfrastructure(this IServiceCollection services)
    {
        var time = Stopwatch.GetTimestamp();
        
        var handlerInterfaceType = typeof(IEventHandler<>);
        var assemblyScanCount = 0;
        var handlerTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly =>
            {
                assemblyScanCount++;
                Log.Debug("Searching for event handlers in {assemblyName}", assembly.FullName);
                return assembly
                    .GetTypes()
                    .Where(t => t is { IsAbstract: false, IsClass: true } && handlerInterfaceType.IsAssignableFrom(t));
            })
            .ToList();
        
        Log.Information("Registered {handlerCount} event handlers from {assemblyCount} assemblies in {ms} milliseconds", 
            handlerTypes.Count, assemblyScanCount, Stopwatch.GetElapsedTime(time).TotalMilliseconds.ToString("N0"));

        foreach (var handlerImplementationType in handlerTypes)
        {
            services.AddScoped(handlerInterfaceType, handlerImplementationType);
        }
        
        return services
            .AddScoped<IPublisher, Publisher>()
            .AddScoped<IEventPublisher, SequentialEventPublisher>();
    } 
}