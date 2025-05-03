using Ardalis.Result;

namespace LedgerLite.Accounting.Domain.Accounts;

internal static class AccountErrors
{
    public const string AccountIdentifier = "Account";

    public static ValidationError NoChildrenWhenNotPlaceholder() => new(
        identifier: AccountIdentifier,
        errorMessage: "Account needs to be a placeholder to have child accounts.",
        errorCode: "ACC-ADD_NOT_PLACEHOLDER",
        severity: ValidationSeverity.Error);

    public static ValidationError ChildHasDifferentType(AccountType expected, AccountType actual) => new(
        identifier: AccountIdentifier,
        errorMessage: $"Expected child account to be {expected}, got {actual} instead.",
        errorCode: "ACC-ADD_WRONG_TYPE",
        severity: ValidationSeverity.Error);

    public static ValidationError AddAccountToItself() => new(
        identifier: AccountIdentifier,
        errorMessage: "Cannot add account as a child of itself.",
        errorCode: "ACC-ADD_TO_SELF",
        severity: ValidationSeverity.Error);
}