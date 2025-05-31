using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;
using LedgerLite.Users.Contracts;

namespace LedgerLite.Accounting.Core.Application.Chart;

public sealed class ChartOfAccountsService(
    IUserRequests userRequests,
    IChartOfAccountsRepository repository) : IChartOfAccountsService
{
    // The ChartOfAccounts is not directly linked to one user but an organization. Therefore, we must first get
    // the organization that the user belongs and then get the ChartOfAccounts.
    public async Task<Result<ChartOfAccounts>> GetByUserIdAsync(Guid userId, CancellationToken token)
    {
        var userResult = await userRequests.GetUserByIdAsync(userId, token);
        if (!userResult.IsSuccess) return userResult.Map();

        var user = userResult.Value;

        if (user.Organization?.Id is not { } organizationId)
            return Result.NotFound("User does not belong in an organization.");

        if (await repository.GetByOrganizationIdAsync(organizationId, token) is not { } chart)
            return Result.NotFound($"Organization with ID '{organizationId}' does not exist.");

        return chart;
    }
}