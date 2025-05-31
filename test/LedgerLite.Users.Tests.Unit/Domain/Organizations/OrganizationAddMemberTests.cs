using Ardalis.Result;
using LedgerLite.Tests.Shared;
using LedgerLite.Users.Application.Roles;
using LedgerLite.Users.Contracts;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Tests.Unit.Utilities;

namespace LedgerLite.Users.Tests.Unit.Domain.Organizations;

public class OrganizationAddMemberTests
{
    [Fact]
    public void AppendMember_ToList()
    {
        var member = FakeOrganizationMembers.Get(x => x.WithRole(CommonRoles.Member));
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

    [Fact]
    public void Invalid_IfAddingAnotherOwner()
    {
        var owner = FakeOrganizationMembers.Get(x => x.WithRole(CommonRoles.Owner));
        var organization = FakeOrganizations.Get(x => x.WithMember(owner));
        var another = FakeOrganizationMembers.Get(x => x.WithRole(CommonRoles.Owner));

        var result = organization.AddMember(another);
        
        result.ShouldBeInvalid();
        result.ShouldHaveError(OrganizationErrors.AlreadyHasOwner());
    }

    [Fact]
    public void Invalid_WhenMember_DoesNotHaveRole()
    {
        var member = FakeOrganizationMembers.Get();
        var organization = FakeOrganizations.Get();

        var result = organization.AddMember(member);
        
        result.ShouldBeInvalid();
        result.ShouldHaveError(OrganizationErrors.MemberDoesNotHaveRole(member));
    }
}