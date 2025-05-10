using Bogus;
using LedgerLite.Tests.Shared;
using LedgerLite.Users.Domain.Organizations;

namespace LedgerLite.Users.Tests.Unit.Utilities;

public static class FakeOrganizations
{
    private static Faker<Organization> Faker(FakeOrganizationBuilder builder) => 
        new PrivateFaker<Organization>(new PrivateBinder())
            .UsePrivateConstructor()
            .RuleFor(x => x.Name, f => builder.Name ?? f.Company.CompanyName())
            .RuleFor("_members", _ => builder.Members);
    
    public static Organization Get(Action<FakeOrganizationBuilder>? configure = null)
    {
        var builder = new FakeOrganizationBuilder();
        configure?.Invoke(builder);
        return Faker(builder).Generate();
    }
}

public sealed record FakeOrganizationBuilder
{
    internal string? Name { get; private set; }
    internal List<OrganizationMember> Members { get; private set; } = [];

    public FakeOrganizationBuilder WithName(string name)
    {
        Name = name;
        return this with { Name = name };
    }
    
    public FakeOrganizationBuilder WithMember(OrganizationMember member)
    {
        Members.Add(member);
        return this;
    }
}