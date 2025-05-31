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
        var userResult = await userRequests.GetUserByIdAsync(id: request.UserId, token: token);
        if (userResult.Value is not { } user) return userResult.Map();

        return user.Organization is null
            ? new List<FiscalPeriod>()
            : Result.Success(
                value: await repository.GetForOrganizationAsync(organizationId: user.Organization.Id, token: token));
    }
}