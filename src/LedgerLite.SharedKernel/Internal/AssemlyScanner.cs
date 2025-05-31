using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LedgerLite.SharedKernel.Internal;

internal static class AssemblyScanner
{
    public static IServiceCollection RegisterServices(
        IServiceCollection services,
        Action<AssemblyScanStrategyBuilder> configure)
    {
        var time = Stopwatch.GetTimestamp();
        var assemblyScanCount = 0;

        var options = new AssemblyScanStrategyBuilder();
        configure(obj: options);

        var search = options.GetTypeSearchStrategy();
        var implementationTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(selector: assembly =>
            {
                assemblyScanCount++;
                return search.Filter(types: assembly.GetTypes());
            })
            .ToList();

        var beforeCount = services.Count;
        foreach (var implementation in implementationTypes)
        {
            if (options.ShouldRegisterInterface)
            {
                var interfaces = search.GetInterfaces(type: implementation);

                foreach (var @interface in interfaces)
                    services.AddScoped(serviceType: @interface, implementationType: implementation);
            }

            if (options.ShouldRegisterImplementation) services.AddScoped(serviceType: implementation);
        }

        Log.Information(
            messageTemplate:
            "Detected {count} implementations of {interface} from {assemblyCount} assemblies in {ms} milliseconds",
            implementationTypes.Count,
            options.BaseType.Name,
            assemblyScanCount,
            Stopwatch.GetElapsedTime(startingTimestamp: time).TotalMilliseconds.ToString(format: "N0"));

        Log.Information(messageTemplate: "Registered {serviceCount} services for {interface}",
            propertyValue0: services.Count - beforeCount,
            propertyValue1: options.BaseType.Name);

        return services;
    }
}