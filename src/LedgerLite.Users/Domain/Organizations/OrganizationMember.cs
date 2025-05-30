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

    public Result AssignRole(Role role)
    {
        if (_roles.Any(x => x.RoleId == role.Id))
        {
            return Result.Conflict($"Member '{User?.Email}' already has role '{role.Name}'");
        }

        var userRole = new UserRole(role, this);
        
        _roles.Add(userRole);
        return Result.Success();
    }

    public Result RevokeRole(Role role)
    {
        var roleToRemove = _roles.FirstOrDefault(x => x.RoleId == role.Id);
        
        if (roleToRemove is null)
        {
            return Result.NotFound($"Member '{User?.Email}' doesn't have role '{role.Name}'");
        }

        _roles.Remove(roleToRemove);
        return Result.Success();
    } 
}