using Ardalis.Result;

namespace LedgerLite.Tests.Shared;

public static class ResultAssertionExtensions
{
    public static void ShouldHaveError(this Result result, ValidationError error)
    {
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(expected: error);
    }

    public static void ShouldHaveError<TValue>(this Result<TValue> result, ValidationError error)
    {
        result.Map().ShouldHaveError(error: error);
    }

    public static void ShouldBeInvalid(this Result result)
    {
        result.Status.ShouldBe(expected: ResultStatus.Invalid);
    }
}