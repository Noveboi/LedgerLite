using Ardalis.Result;
using LedgerLite.SharedKernel.Domain;
using LedgerLite.SharedKernel.Domain.Errors;
using LedgerLite.Users.Domain.Organizations.Events;

namespace LedgerLite.Users.Domain.Organizations;

/// <summary>
/// An organization can be one person, a small business or an enterprise. It is simply a collection of user under a
/// common entity.
/// </summary>
public sealed class Organization : AuditableEntity
{
    public string Name { get; private set; } = null!;

    private readonly List<OrganizationMember> _members = [];

    private Organization() { }
    private Organization(string name)
    {
        Name = name;
    }

    public IReadOnlyCollection<OrganizationMember> Members => _members;

    public static Result<Organization> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Invalid(CommonErrors.NameIsEmpty());

        var organization = new Organization(name);
        organization.AddDomainEvent(new OrganizationCreatedEvent(organization.Id, name));
        
        return Result.Success(organization);
    }

    public Result AddMember(OrganizationMember member)
    {
        if (_members.Any(m => m == member))
            return Result.Invalid(OrganizationErrors.MemberAlreadyInOrganization(member));
        
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