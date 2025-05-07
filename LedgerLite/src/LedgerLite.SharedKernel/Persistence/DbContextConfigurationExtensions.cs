using LedgerLite.SharedKernel.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LedgerLite.SharedKernel.Persistence;

public static class DbContextConfigurationExtensions
{
    public static DbContextOptionsBuilder AddAuditLogging(this DbContextOptionsBuilder options) => 
        options.AddInterceptors(new TimeAuditInterceptor());
}