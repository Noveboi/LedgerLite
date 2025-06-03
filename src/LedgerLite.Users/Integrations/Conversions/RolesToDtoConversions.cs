using LedgerLite.Users.Contracts.Models;
using LedgerLite.Users.Domain;

namespace LedgerLite.Users.Integrations.Conversions;

internal static class RolesToDtoConversions
{
    public static RoleDto ToDto(this Role role)
    {
        return new RoleDto(
            Id: role.Id,
            role.Name ?? "");
    }
}