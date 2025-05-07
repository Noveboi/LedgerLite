using LedgerLite.SharedKernel.Events;
using LedgerLite.SharedKernel.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LedgerLite.SharedKernel.Persistence;

public static class DbContextConfigurationExtensions
{
    public static DbContextOptionsBuilder AddAuditLogging(this DbContextOptionsBuilder options) => 
        options.AddInterceptors(new TimeAuditInterceptor());

    public static DbContextOptionsBuilder AddDomainEventProcessing(
        this DbContextOptionsBuilder options,
        IServiceProvider sp)
    {
        return options.AddInterceptors(new DomainEventInterceptor(sp.GetRequiredService<IPublisher>()));
    }
}