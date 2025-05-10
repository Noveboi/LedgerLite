using Ardalis.Result;
using LedgerLite.Tests.Shared;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Tests.Unit.Utilities;

namespace LedgerLite.Users.Tests.Unit.Domain.Organizations;

public class OrganizationRemoveMemberTests
{
    [Fact]
    public void RemoveMember_FromList()
    {
        var member = FakeOrganizationMembers.Get();
        var organization = FakeOrganizations.Get(x => x.WithMember(member));

        var result = organization.RemoveMember(member);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        organization.Members.ShouldBeEmpty();
    }

    [Fact]
    public void Invalid_IfMemberDoesNotExistInOrganization()
    {
        var member = FakeOrganizationMembers.Get();
        var organization = FakeOrganizations.Get();

        var result = organization.RemoveMember(member);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ShouldHaveError(OrganizationErrors.MemberNotInOrganization(member));
    }
}