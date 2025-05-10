using Ardalis.Result;
using LedgerLite.Tests.Shared;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Tests.Unit.Utilities;

namespace LedgerLite.Users.Tests.Unit.Domain.Organizations;

public class OrganizationAddMemberTests
{
    [Fact]
    public void AppendMember_ToList()
    {
        var member = FakeOrganizationMembers.Get();
        var organization = FakeOrganizations.Get();

        var result = organization.AddMember(member);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        organization.Members.ShouldHaveSingleItem().ShouldBeEquivalentTo(member);
    }

    [Fact]
    public void Invalid_WhenMemberAlreadyInOrganization()
    {
        var member = FakeOrganizationMembers.Get();
        var organization = FakeOrganizations.Get(o => o.WithMember(member));

        var result = organization.AddMember(member);
        
        result.ShouldBeInvalid();
        result.ShouldHaveError(OrganizationErrors.MemberAlreadyInOrganization(member));
    }
}