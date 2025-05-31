using LedgerLite.SharedKernel.Events;
using LedgerLite.SharedKernel.Extensions;
using LedgerLite.SharedKernel.UseCases;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LedgerLite.SharedKernel;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedKernelServices(this IServiceCollection services)
    {
        Log.Logger.RegisteringModule(moduleName: "Shared Kernel");
        return services
            .AddApplicationUseCases()
            .AddEventInfrastructure();
    }
}