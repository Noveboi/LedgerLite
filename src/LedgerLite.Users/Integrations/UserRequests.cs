using Ardalis.Result;
using LedgerLite.Users.Application.Users;
using LedgerLite.Users.Contracts;
using LedgerLite.Users.Contracts.Models;
using LedgerLite.Users.Infrastructure.Repositories;
using LedgerLite.Users.Integrations.Conversions;

namespace LedgerLite.Users.Integrations;

internal sealed class UserRequests(IUserService userService, IOrganizationRepository org) : IUserRequests
{
    public async Task<Result<UserDto>> GetUserByIdAsync(Guid id, CancellationToken token)
    {
        var userResult = await userService.GetByIdAsync(id: id, token: token);
        if (!userResult.IsSuccess) return userResult.Map();

        var user = userResult.Value;

        var organization = user.OrganizationMember?.OrganizationId is { } orgId
            ? await org.GetByIdAsync(id: orgId, token: token)
            : null;

        return user.ToDto(organization: organization);
    }

    public async Task<bool> UserBelongsInOrganizationAsync(Guid userId, Guid organizationId, CancellationToken token)
    {
        var userResult = await userService.GetByIdAsync(id: userId, token: token);
        if (!userResult.IsSuccess) return false;

        var user = userResult.Value;
        return organizationId == user.OrganizationMember?.OrganizationId;
    }
}