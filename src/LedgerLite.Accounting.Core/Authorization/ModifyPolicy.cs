using LedgerLite.Users.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace LedgerLite.Accounting.Core.Authorization;

internal static class ModifyPolicy
{
    public static readonly IEnumerable<string> AllowedRoles =
    [
        CommonRoles.Member,
        CommonRoles.Admin,
        CommonRoles.Owner
    ];

    public static AuthorizationPolicyBuilder RequireModificationPermissions(this AuthorizationPolicyBuilder builder)
    {
        return builder.RequireRole(AllowedRoles);
    }
}