using Ardalis.Result;
using LedgerLite.SharedKernel.Domain;
using LedgerLite.SharedKernel.Domain.Errors;
using LedgerLite.Users.Contracts;
using LedgerLite.Users.Domain.Organizations.Events;

namespace LedgerLite.Users.Domain.Organizations;

/// <summary>
///     An organization can be one person, a small business or an enterprise. It is simply a collection of users under a
///     common entity.
/// </summary>
public sealed class Organization : AuditableEntity
{
    private readonly List<OrganizationMember> _members = [];

    private Organization()
    {
    }

    private Organization(string name)
    {
        Name = name;
    }

    public string Name { get; private set; } = null!;
    public IReadOnlyCollection<OrganizationMember> Members => _members;

    public static Result<Organization> Create(User creator, Role creatorRole, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Invalid(CommonErrors.NameIsEmpty());

        if (creatorRole.Name != CommonRoles.Owner)
            throw new InvalidOperationException($"Organization creator must be '{CommonRoles.Owner}'.");

        var organization = new Organization(name);
        var member = OrganizationMember.Create(creator, organization, creatorRole);
        organization.AddMember(member);

        organization.AddDomainEvent(new OrganizationCreatedEvent(organization));

        return Result.Success(organization);
    }

    public Result AddMember(OrganizationMember member)
    {
        if (_members.Any(m => m == member))
            return Result.Invalid(OrganizationErrors.MemberAlreadyInOrganization(member));

        if (member.Roles.Count == 0)
            return Result.Invalid(OrganizationErrors.MemberDoesNotHaveRole(member));

        if (_members.Any(m => m.HasRole(CommonRoles.Owner)) && member.HasRole(CommonRoles.Owner))
            return Result.Invalid(OrganizationErrors.AlreadyHasOwner());

        _members.Add(member);
        return Result.Success();
    }

    public Result RemoveMember(OrganizationMember member)
    {
        return _members.Remove(member)
            ? Result.Success()
            : Result.Invalid(OrganizationErrors.MemberNotInOrganization(member));
    }

    public Result Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            return Result.Invalid(CommonErrors.NameIsEmpty());

        if (newName == Name)
            return Result.Invalid(OrganizationErrors.NameIsTheSame());

        Name = newName;
        return Result.Success();
    }
}