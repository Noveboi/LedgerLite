using Ardalis.Result;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.Users.Domain.Organization;

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

    public Result AddMember(OrganizationMember member)
    {
        throw new NotImplementedException();
    }

    public Result RemoveMember(OrganizationMember member)
    {
        throw new NotImplementedException();
    }

    public Result Rename(string newName)
    {
        throw new NotImplementedException();
    }
}