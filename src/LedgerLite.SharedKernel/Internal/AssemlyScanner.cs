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
        configure(options);

        var search = options.GetTypeSearchStrategy();
        var implementationTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly =>
            {
                assemblyScanCount++;
                return search.Filter(assembly.GetTypes());
            })
            .ToList();

        var beforeCount = services.Count;
        foreach (var implementation in implementationTypes)
        {
            if (options.ShouldRegisterInterface)
            {
                var interfaces = search.GetInterfaces(implementation);

                foreach (var @interface in interfaces)
                {
                    services.AddScoped(@interface, implementation);
                }
            }

            if (options.ShouldRegisterImplementation)
            {
                services.AddScoped(implementation);
            }
        }
        
        Log.Information("Detected {count} implementations of {interface} from {assemblyCount} assemblies in {ms} milliseconds", 
            implementationTypes.Count,
            options.BaseType.Name,
            assemblyScanCount, 
            Stopwatch.GetElapsedTime(time).TotalMilliseconds.ToString("N0"));
        
        Log.Information("Registered {serviceCount} services for {interface}",
            services.Count - beforeCount,
            options.BaseType.Name);

        return services;
    }
}