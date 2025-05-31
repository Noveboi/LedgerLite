using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LedgerLite.SharedKernel.Persistence;

public abstract class UnitOfWork<TContext>(TContext context) : IUnitOfWork where TContext : DbContext
{
    private static readonly ILogger Logger = Log.ForContext<UnitOfWork<TContext>>();

    public async Task<Result> SaveChangesAsync(CancellationToken token)
    {
        try
        {
            await context.SaveChangesAsync(cancellationToken: token);
            return Result.Success();
        }
        catch (DbUpdateException exception)
        {
            Logger.Error(exception: exception, messageTemplate: "Save Changes ERROR");
            return Result.Error(errorMessage: "Something went wrong.");
        }
        catch (Exception exception)
        {
            Logger.Error(exception: exception, messageTemplate: "Save Changes UNKNOWN ERROR!");
            return Result.Error(errorMessage: "Something went wrong.");
        }
    }
}