using Ardalis.Result;

namespace LedgerLite.SharedKernel.UseCases;

public interface IApplicationUseCase<in TRequest, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken token);
}

public interface IApplicationUseCase<in TRequest>
{
    Task<Result> HandleAsync(TRequest request, CancellationToken token);
}