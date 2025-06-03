using Ardalis.Result;
using LedgerLite.SharedKernel.Domain.Errors;
using LedgerLite.Tests.Shared;
using LedgerLite.Users.Contracts;
using LedgerLite.Users.Domain;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Tests.Unit.Utilities;

namespace LedgerLite.Users.Tests.Unit.Domain.Organizations;

public class OrganizationCreateTests
{
    private static readonly Role Role = new(name: CommonRoles.Owner);
    private static readonly User User = FakeUsers.Get();

    [Fact]
    public void Invalid_WhenNameIsEmptyOrWhitespace()
    {
        var result = Organization.Create(creator: User, creatorRole: Role, name: "");

        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ShouldHaveError(CommonErrors.NameIsEmpty());
    }

    [Fact]
    public void Create_WhenNameIsValid()
    {
        var result = Organization.Create(creator: User, creatorRole: Role, name: "Test Org!");

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        result.Value.Name.ShouldBe(expected: "Test Org!");
        result.Value.Members.ShouldHaveSingleItem();
    }

    [Fact]
    public void ThrowException_IfRoleIsNotOwner()
    {
        var role = new Role(name: "Ok!");

        var action = () => Organization.Create(creator: User, creatorRole: role, name: "AAAA");

        action.ShouldThrow<InvalidOperationException>();
    }

    [Fact]
    public void AddCreator_AsOrganizationMember()
    {
        var result = Organization.Create(creator: User, creatorRole: Role, name: "Abc!");
        var org = result.Value;

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        var member = org.Members.ShouldHaveSingleItem();
        member.User.ShouldBe(expected: User);
        member.OrganizationId.ShouldBe(expected: org.Id);
        member.Roles.ShouldHaveSingleItem().RoleId.ShouldBe(expected: Role.Id);
    }
}