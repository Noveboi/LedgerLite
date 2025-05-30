using LedgerLite.Users.Domain.Organizations;
using Microsoft.AspNetCore.Identity;

namespace LedgerLite.Users.Domain;

public sealed class UserRole : IdentityUserRole<Guid>
{
    public Role Role { get; set; } = null!;
    public Guid OrganizationMemberId { get; set; }
    public OrganizationMember OrganizationMember { get; set; } = null!;
}