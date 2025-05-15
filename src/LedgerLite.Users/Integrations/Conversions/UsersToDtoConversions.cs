using LedgerLite.Users.Contracts.Models;
using LedgerLite.Users.Domain;

namespace LedgerLite.Users.Integrations.Conversions;

internal static class UsersToDtoConversions
{
    public static UserDto ToDto(this User user) => new(
        Id: user.Id,
        OrganizationId: user.OrganizationMember?.OrganizationId,
        Username: user.UserName ?? "",
        Email: user.Email ?? "",
        FullName: (user.FirstName, user.LastName) switch
        {
            (not null, not null) => $"{user.FirstName} {user.LastName}",
            (null, not null) => user.LastName,
            _ => ""
        });
}