using Serilog;

namespace LedgerLite.SharedKernel.Extensions;

public static class LoggingExtensions
{
    public static void RegisteringModule(this ILogger logger, string moduleName)
    {
        logger.Information("Registering '{module}' module", moduleName);
    }
}