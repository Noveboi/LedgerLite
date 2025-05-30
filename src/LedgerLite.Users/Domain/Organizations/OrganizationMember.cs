using Ardalis.Result;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.Users.Domain.Organizations;

/// <summary>
/// A user inside an organization. As an organization member, a user has a specific role.
/// </summary>
public sealed class OrganizationMember : AuditableEntity
{
    private OrganizationMember() { }
    private OrganizationMember(User user, Guid organizationId)
    {
        OrganizationId = organizationId;
        UserId = user.Id;
    }

    public Guid OrganizationId { get; private set; } 
    public Guid UserId { get; private set; }
    public User User { get; private init; } = null!;


    private readonly List<UserRole> _roles = [];
    public IReadOnlyCollection<UserRole> Roles => _roles;

    public static Result<OrganizationMember> Create(User user, Guid organizationId) => 
        Result.Success(new OrganizationMember(
            user: user,
            organizationId: organizationId));
}