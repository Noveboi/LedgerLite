using Ardalis.Result;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.Users.Domain.Organizations;

/// <summary>
/// A user inside an organization. As an organization member, a user has a specific role.
/// </summary>
public sealed class OrganizationMember : AuditableEntity
{
    private OrganizationMember() { }
    private OrganizationMember(User user, OrganizationMemberRole role, Guid organizationId)
    {
        OrganizationId = organizationId;
        User = user;
        Role = role;
    }

    public Guid OrganizationId { get; private set; } 

    public User User { get; private init; } = null!;
    public OrganizationMemberRole Role { get; private set; } = null!;

    public static Result<OrganizationMember> Create(User user, OrganizationMemberRole role, Guid organizationId) => 
        Result.Success(new OrganizationMember(
            user: user,
            role: role,
            organizationId: organizationId));
    
    /// <summary>
    /// Admin/Owner only 
    /// </summary>
    public Result PromoteMember(OrganizationMember promotee, OrganizationMemberRole newRole)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Admin/Owner only 
    /// </summary>
    public Result DemoteMember(OrganizationMember demotee, OrganizationMemberRole newRole)
    {
        throw new NotImplementedException();
    }
}