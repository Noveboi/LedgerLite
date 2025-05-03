namespace LedgerLite.SharedKernel.Persistence;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken token);
}