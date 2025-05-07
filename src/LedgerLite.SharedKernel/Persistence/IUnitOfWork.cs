using Ardalis.Result;

namespace LedgerLite.SharedKernel.Persistence;

public interface IUnitOfWork
{
    Task<Result> SaveChangesAsync(CancellationToken token);
}