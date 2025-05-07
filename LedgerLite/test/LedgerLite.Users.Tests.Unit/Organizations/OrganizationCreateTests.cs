using Ardalis.Result;
using LedgerLite.SharedKernel.Domain.Errors;
using LedgerLite.Users.Domain.Organizations;

namespace LedgerLite.Users.Tests.Unit.Organizations;

public class OrganizationCreateTests
{
    [Fact]
    public void Invalid_WhenNameIsEmptyOrWhitespace()
    {
        var result = Organization.Create("");
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(CommonErrors.NameIsEmpty());
    }

    [Fact]
    public void Create_WhenNameIsValid()
    {
        var result = Organization.Create("Test Org!");
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.Name.ShouldBe("Test Org!");
        result.Value.Members.ShouldBeEmpty();
    }
}