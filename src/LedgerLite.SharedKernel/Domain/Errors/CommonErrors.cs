using Ardalis.Result;
using Humanizer;

namespace LedgerLite.SharedKernel.Domain.Errors;

public static class CommonErrors
{
    private const string CommonIdentifier = "Common";

    public static ValidationError NameIsEmpty() => new(
        identifier: CommonIdentifier,
        errorMessage: "Name is empty.",
        errorCode: "NAME_EMPTY",
        severity: ValidationSeverity.Error);

    public static string NotFound<T>(Guid id) where T : class
    {
        var name = typeof(T).Name;
        return $"{name.Humanize(LetterCasing.Title)} with ID '{id}' does not exist";
    }
}