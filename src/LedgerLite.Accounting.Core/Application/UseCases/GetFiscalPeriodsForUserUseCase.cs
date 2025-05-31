using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;
using LedgerLite.SharedKernel.UseCases;
using LedgerLite.Users.Contracts;

namespace LedgerLite.Accounting.Core.Application.UseCases;

internal sealed record GetFiscalPeriodsForUserRequest(Guid UserId);

internal sealed class GetFiscalPeriodsForUserUseCase(
    IUserRequests userRequests,
    IFiscalPeriodRepository repository)
    : IApplicationUseCase<GetFiscalPeriodsForUserRequest, IReadOnlyList<FiscalPeriod>>
{
    public async Task<Result<IReadOnlyList<FiscalPeriod>>> HandleAsync(
        GetFiscalPeriodsForUserRequest request,
        CancellationToken token)
    {
        var userResult = await userRequests.GetUserByIdAsync(request.UserId, token);
        if (userResult.Value is not { } user) return userResult.Map();

        return user.Organization is null
            ? new List<FiscalPeriod>()
            : Result.Success(await repository.GetForOrganizationAsync(user.Organization.Id, token));
    }
}