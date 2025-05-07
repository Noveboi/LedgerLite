using Ardalis.Result;
using LedgerLite.SharedKernel.Domain;
using LedgerLite.SharedKernel.Domain.Errors;

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

        return Result.Success(new Organization(name));
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
        throw new NotImplementedException();
    }
}