using Ardalis.Result;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.Users.Domain.Organizations;

/// <summary>
///     A user inside an organization. As an organization member, a user has a specific role.
/// </summary>
public sealed class OrganizationMember : AuditableEntity
{
    private readonly List<UserRole> _roles = null!;

    private OrganizationMember()
    {
    }

    private OrganizationMember(User user, Guid organizationId, Role role)
    {
        OrganizationId = organizationId;
        User = user;
        _roles = [new UserRole(role: role, this)];
    }

    public Organization Organization { get; private init; } = null!;
    public Guid OrganizationId { get; private set; }
    public User User { get; private init; } = null!;
    public IReadOnlyCollection<UserRole> Roles => _roles;

    public static Result<OrganizationMember> Create(User user, Organization organization, Role role)
    {
        return Result.Success(new OrganizationMember(
            user: user,
            organizationId: organization.Id,
            role: role));
    }

    public bool HasRole(string name)
    {
        return _roles.Any(x => x.Role.Name == name);
    }
}