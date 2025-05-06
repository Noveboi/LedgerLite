using System.Diagnostics.CodeAnalysis;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LedgerLite.Accounting.Core.Infrastructure;

internal sealed class AccountingUnitOfWork(
    IServiceProvider serviceProvider,
    AccountingDbContext context) : IAccountingUnitOfWork
{
    private static readonly ILogger Logger = Log.ForContext<AccountingUnitOfWork>();
    
    public async Task<Result> SaveChangesAsync(CancellationToken token)
    {
        try
        {
            await context.SaveChangesAsync(token);
            return Result.Success();
        }
        catch (DbUpdateException exception)
        {
            Logger.Error(exception, "Save Changes ERROR");
            return Result.Error("Something went wrong.");
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Save Changes UNKNOWN ERROR!");
            return Result.Error("Something went wrong.");
        }
    }

    [field: AllowNull, MaybeNull]
    public IAccountRepository AccountRepository => 
        field ?? serviceProvider.GetRequiredService<IAccountRepository>();

    [field: AllowNull, MaybeNull]
    public IChartOfAccountsRepository ChartOfAccountsRepository =>
        field ?? serviceProvider.GetRequiredService<IChartOfAccountsRepository>();

    [field: AllowNull, MaybeNull]
    public IJournalEntryRepository JournalEntryRepository =>
        field ?? serviceProvider.GetRequiredService<IJournalEntryRepository>();

    [field: AllowNull, MaybeNull]
    public IJournalEntryLineRepository JournalEntryLineRepository =>
        field ?? serviceProvider.GetRequiredService<IJournalEntryLineRepository>();
}