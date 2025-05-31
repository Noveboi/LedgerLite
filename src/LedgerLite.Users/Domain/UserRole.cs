using LedgerLite.Users.Domain.Organizations;
using Microsoft.AspNetCore.Identity;

namespace LedgerLite.Users.Domain;

public sealed class UserRole : IdentityUserRole<Guid>
{
    public UserRole() { }
    public UserRole(Role role, OrganizationMember member)
    {
        RoleId = role.Id;
        OrganizationMemberId = member.Id;
        UserId = member.User.Id;
    }

    public Role Role { get; init; } = null!;
    public Guid OrganizationMemberId { get; init; }
    public OrganizationMember OrganizationMember { get; init; } = null!;
}