using Ardalis.Result;

namespace LedgerLite.SharedKernel.Domain.Errors;

public static class CommonErrors
{
    public const string CommonIdentifier = "Common";

    public static ValidationError NameIsEmpty() => new(
        identifier: CommonIdentifier,
        errorMessage: "Name is empty.",
        errorCode: "NAME_EMPTY",
        severity: ValidationSeverity.Error);
}