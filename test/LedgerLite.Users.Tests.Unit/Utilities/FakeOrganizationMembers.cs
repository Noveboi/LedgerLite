using Bogus;
using LedgerLite.Tests.Shared;
using LedgerLite.Users.Domain;
using LedgerLite.Users.Domain.Organizations;

namespace LedgerLite.Users.Tests.Unit.Utilities;

public static class FakeOrganizationMembers
{
    private static Faker<OrganizationMember> Faker(FakeOrganizationMemberBuilder builder) =>
        new PrivateFaker<OrganizationMember>()
            .UsePrivateConstructor()
            .RuleFor(x => x.OrganizationId, _ => builder.OrganizationId ?? Guid.NewGuid())
            .RuleFor(x => x.User, _ => builder.User ?? FakeUsers.Get());

    public static OrganizationMember Get(Action<FakeOrganizationMemberBuilder>? configure = null)
    {
        var builder = new FakeOrganizationMemberBuilder();
        configure?.Invoke(builder);
        return Faker(builder);
    }
}

public sealed class FakeOrganizationMemberBuilder
{
    internal User? User { get; private set; }
    internal Guid? OrganizationId { get; private set; }

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