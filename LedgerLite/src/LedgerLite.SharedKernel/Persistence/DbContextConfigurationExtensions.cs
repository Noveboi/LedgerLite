using LedgerLite.SharedKernel.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.SharedKernel.Persistence;

public static class DbContextConfigurationExtensions
{
    public static DbContextOptionsBuilder AddAuditLogging(this DbContextOptionsBuilder options) 
    {
        return options.AddInterceptors(new TimeAuditInterceptor());
    }
}