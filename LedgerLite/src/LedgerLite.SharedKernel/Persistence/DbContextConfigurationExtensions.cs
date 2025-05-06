using LedgerLite.SharedKernel.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LedgerLite.SharedKernel.Persistence;

public static class DbContextConfigurationExtensions
{
    private static bool _hasLogged = false;
    public static DbContextOptionsBuilder AddAuditLogging(this DbContextOptionsBuilder options) 
    {
        if (!_hasLogged)
        {
            Log.Information("Adding audit logging for {context}", options.Options.ContextType.Name);
            _hasLogged = true;
        }
        return options.AddInterceptors(new TimeAuditInterceptor());
    }
}