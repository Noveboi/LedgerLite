using LedgerLite.SharedKernel.Internal;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LedgerLite.SharedKernel.UseCases;

internal static class DependencyInjection
{
    public static IServiceCollection AddApplicationUseCases(this IServiceCollection services)
    {
        Log.Information(messageTemplate: "Configuring {name}", propertyValue: "Application Use Cases");
        return AssemblyScanner.RegisterServices(services: services, configure: o => o
            .Implementing(type: typeof(IApplicationUseCase<,>))
            .RegisterImplementationOnly());
    }
}