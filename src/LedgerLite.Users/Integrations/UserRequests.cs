using Ardalis.Result;
using LedgerLite.Users.Application.Organizations;
using LedgerLite.Users.Contracts;
using LedgerLite.Users.Contracts.Models;
using LedgerLite.Users.Domain;
using LedgerLite.Users.Infrastructure.Repositories;
using LedgerLite.Users.Integrations.Conversions;
using Microsoft.AspNetCore.Identity;

namespace LedgerLite.Users.Integrations;

internal sealed class UserRequests(UserManager<User> userManager, IOrganizationRepository org) : IUserRequests
{
    public async Task<Result<UserDto>> GetUserByIdAsync(Guid id, CancellationToken token)
    {
        if (await userManager.FindByIdAsync(id.ToString()) is not { } user)
        {
            return Result.NotFound($"Couldn't find user with ID '{id}'");
        }

        var organization = user.OrganizationMember?.OrganizationId is { } orgId
            ? await org.GetByIdAsync(orgId, token)
            : null;
        
        return user.ToDto(organization);
    }
}