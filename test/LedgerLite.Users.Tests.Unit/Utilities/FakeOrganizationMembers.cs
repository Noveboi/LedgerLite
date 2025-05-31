using Bogus;
using LedgerLite.Tests.Shared;
using LedgerLite.Users.Domain;
using LedgerLite.Users.Domain.Organizations;

namespace LedgerLite.Users.Tests.Unit.Utilities;

public static class FakeOrganizationMembers
{
    private static Faker<OrganizationMember> Faker(FakeOrganizationMemberBuilder builder)
    {
        return new PrivateFaker<OrganizationMember>(binder: new PrivateBinder())
            .UsePrivateConstructor()
            .RuleFor(property: x => x.OrganizationId, setter: _ => builder.OrganizationId ?? Guid.NewGuid())
            .RuleFor(property: x => x.User, setter: _ => builder.User ?? FakeUsers.Get())
            .RuleFor(propertyOrFieldName: "_roles",
                setter: (_, m) => builder.Roles.Select(selector: func => func(arg: m)).ToList());
    }

    public static OrganizationMember Get(Action<FakeOrganizationMemberBuilder>? configure = null)
    {
        var builder = new FakeOrganizationMemberBuilder();
        configure?.Invoke(obj: builder);
        return Faker(builder: builder);
    }
}

public sealed class FakeOrganizationMemberBuilder
{
    internal User? User { get; private set; }
    internal Guid? OrganizationId { get; private set; }
    internal List<Func<OrganizationMember, UserRole>> Roles { get; } = [];

    public FakeOrganizationMemberBuilder WithRole(string name)
    {
        var role = new Role(name: name);
        Roles.Add(item: m => new UserRole
        {
            OrganizationMember = m,
            OrganizationMemberId = m.Id,
            UserId = m.User.Id,
            RoleId = role.Id,
            Role = role
        });
        return this;
    }

    public FakeOrganizationMemberBuilder WithUser(User user)
    {
        User = user;
        return this;
    }

    public FakeOrganizationMemberBuilder InOrganization(Guid id)
    {
        OrganizationId = id;
        return this;
    }
}