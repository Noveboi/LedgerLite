using Ardalis.Result;
using LedgerLite.SharedKernel.Domain.Errors;
using LedgerLite.Tests.Shared;
using LedgerLite.Users.Application.Roles;
using LedgerLite.Users.Domain;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Tests.Unit.Utilities;

namespace LedgerLite.Users.Tests.Unit.Domain.Organizations;

public class OrganizationCreateTests
{
    private static readonly Role Role = new(CommonRoles.Owner);
    private static readonly User User = FakeUsers.Get();
    
    [Fact]
    public void Invalid_WhenNameIsEmptyOrWhitespace()
    {
        var result = Organization.Create(User, Role, "");
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ShouldHaveError(CommonErrors.NameIsEmpty());
    }

    [Fact]
    public void Create_WhenNameIsValid()
    {
        var result = Organization.Create(User, Role, "Test Org!");
        
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.Name.ShouldBe("Test Org!");
        result.Value.Members.ShouldHaveSingleItem();
    }

    [Fact]
    public void ThrowException_IfRoleIsNotOwner()
    {
        var role = new Role("Ok!");

        var action = () => Organization.Create(User, role, "AAAA");

        action.ShouldThrow<InvalidOperationException>();
    }

    [Fact]
    public void AddCreator_AsOrganizationMember()
    {
        var result = Organization.Create(User, Role, "Abc!");
        var org = result.Value;
        
        result.Status.ShouldBe(ResultStatus.Ok);
        var member = org.Members.ShouldHaveSingleItem();
        member.User.ShouldBe(User);
        member.OrganizationId.ShouldBe(org.Id);
        member.Roles.ShouldHaveSingleItem().RoleId.ShouldBe(Role.Id);
    }
}