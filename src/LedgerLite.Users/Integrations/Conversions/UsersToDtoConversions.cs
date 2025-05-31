using LedgerLite.Users.Contracts.Models;
using LedgerLite.Users.Domain;
using LedgerLite.Users.Domain.Organizations;

namespace LedgerLite.Users.Integrations.Conversions;

internal static class UsersToDtoConversions
{
    public static UserDto ToDto(this User user, Organization? organization)
    {
        if (user.OrganizationMember?.OrganizationId is { } id && organization?.Id != id)
            throw new ArgumentException("Organization does not match ID of user's organization", nameof(organization));
                
        return new UserDto(
            Id: user.Id,
            Organization: organization?.ToDto(),
            OrganizationRoles: user.OrganizationMember?.Roles
                .Where(x => x.Role.Name is not null)
                .Select(x => x.Role.ToDto()) ?? [],
            Username: user.UserName ?? "",
            Email: user.Email ?? "",
            FullName: (user.FirstName, user.LastName) switch
            {
                (not null, not null) => $"{user.FirstName} {user.LastName}",
                (null, not null) => user.LastName,
                _ => ""
            });
    }
}