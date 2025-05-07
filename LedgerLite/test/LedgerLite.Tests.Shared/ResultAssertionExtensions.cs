using Ardalis.Result;

namespace LedgerLite.Tests.Shared;

public static class ResultAssertionExtensions
{
    public static void ShouldHaveError(this Result result, ValidationError error) => result.ValidationErrors
        .ShouldHaveSingleItem()
        .ShouldBeEquivalentTo(error);

    public static void ShouldBeInvalid(this Result result) => result.Status.ShouldBe(ResultStatus.Invalid);
}