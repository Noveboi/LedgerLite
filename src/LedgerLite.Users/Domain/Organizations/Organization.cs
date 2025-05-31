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
        if (string.IsNullOrWhiteSpace(value: name))
            return Result.Invalid(validationError: CommonErrors.NameIsEmpty());

        if (creatorRole.Name != CommonRoles.Owner)
            throw new InvalidOperationException(message: $"Organization creator must be '{CommonRoles.Owner}'.");

        var organization = new Organization(name: name);
        var member = OrganizationMember.Create(user: creator, organization: organization, role: creatorRole);
        organization.AddMember(member: member);

        organization.AddDomainEvent(domainEvent: new OrganizationCreatedEvent(org: organization));

        return Result.Success(value: organization);
    }

    public Result AddMember(OrganizationMember member)
    {
        if (_members.Any(predicate: m => m == member))
            return Result.Invalid(validationError: OrganizationErrors.MemberAlreadyInOrganization(member: member));

        if (member.Roles.Count == 0)
            return Result.Invalid(validationError: OrganizationErrors.MemberDoesNotHaveRole(member: member));

        if (_members.Any(predicate: m => m.HasRole(name: CommonRoles.Owner)) && member.HasRole(name: CommonRoles.Owner))
            return Result.Invalid(validationError: OrganizationErrors.AlreadyHasOwner());

        _members.Add(item: member);
        return Result.Success();
    }

    public Result RemoveMember(OrganizationMember member)
    {
        return _members.Remove(item: member)
            ? Result.Success()
            : Result.Invalid(validationError: OrganizationErrors.MemberNotInOrganization(member: member));
    }

    public Result Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(value: newName))
            return Result.Invalid(validationError: CommonErrors.NameIsEmpty());

        if (newName == Name)
            return Result.Invalid(validationError: OrganizationErrors.NameIsTheSame());

        Name = newName;
        return Result.Success();
    }
}