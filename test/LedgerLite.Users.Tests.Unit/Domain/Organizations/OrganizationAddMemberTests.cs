﻿using Ardalis.Result;
using LedgerLite.Tests.Shared;
using LedgerLite.Users.Contracts;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Tests.Unit.Utilities;

namespace LedgerLite.Users.Tests.Unit.Domain.Organizations;

public class OrganizationAddMemberTests
{
    [Fact]
    public void AppendMember_ToList()
    {
        var member = FakeOrganizationMembers.Get(x => x.WithRole(name: CommonRoles.Member));
        var organization = FakeOrganizations.Get();

        var result = organization.AddMember(member: member);

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        organization.Members.ShouldHaveSingleItem().ShouldBeEquivalentTo(expected: member);
    }

    [Fact]
    public void Invalid_WhenMemberAlreadyInOrganization()
    {
        var member = FakeOrganizationMembers.Get();
        var organization = FakeOrganizations.Get(o => o.WithMember(member: member));

        var result = organization.AddMember(member: member);

        result.ShouldBeInvalid();
        result.ShouldHaveError(OrganizationErrors.MemberAlreadyInOrganization(member: member));
    }

    [Fact]
    public void Invalid_IfAddingAnotherOwner()
    {
        var owner = FakeOrganizationMembers.Get(x => x.WithRole(name: CommonRoles.Owner));
        var organization = FakeOrganizations.Get(x => x.WithMember(member: owner));
        var another = FakeOrganizationMembers.Get(x => x.WithRole(name: CommonRoles.Owner));

        var result = organization.AddMember(member: another);

        result.ShouldBeInvalid();
        result.ShouldHaveError(OrganizationErrors.AlreadyHasOwner());
    }

    [Fact]
    public void Invalid_WhenMember_DoesNotHaveRole()
    {
        var member = FakeOrganizationMembers.Get();
        var organization = FakeOrganizations.Get();

        var result = organization.AddMember(member: member);

        result.ShouldBeInvalid();
        result.ShouldHaveError(OrganizationErrors.MemberDoesNotHaveRole(member: member));
    }
}