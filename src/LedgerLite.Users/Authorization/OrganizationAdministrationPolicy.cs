using LedgerLite.Users.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace LedgerLite.Users.Authorization;

internal static class OrganizationAdministrationPolicy
{
    public static AuthorizationPolicyBuilder RequireOrganizationAdministrator(this AuthorizationPolicyBuilder builder) =>
        builder.RequireRole(CommonRoles.Owner, CommonRoles.Admin);
}