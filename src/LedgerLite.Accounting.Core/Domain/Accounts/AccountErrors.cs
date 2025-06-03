using Ardalis.Result;

namespace LedgerLite.Accounting.Core.Domain.Accounts;

internal static class AccountErrors
{
    public const string AccountIdentifier = "Account";

    public static ValidationError NoChildrenWhenNotPlaceholder(Account account)
    {
        return new ValidationError(
            identifier: AccountIdentifier,
            $"Account '{account}' needs to be a placeholder to have child accounts.",
            errorCode: "ACC-ADD_NOT_PLACEHOLDER",
            severity: ValidationSeverity.Error);
    }

    public static ValidationError ChildHasDifferentType(AccountType expected, AccountType actual)
    {
        return new ValidationError(
            identifier: AccountIdentifier,
            $"Expected child account to be {expected}, got {actual} instead.",
            errorCode: "ACC-ADD_WRONG_TYPE",
            severity: ValidationSeverity.Error);
    }

    public static ValidationError AddAccountToItself()
    {
        return new ValidationError(
            identifier: AccountIdentifier,
            errorMessage: "Cannot add account as a child of itself.",
            errorCode: "ACC-ADD_TO_SELF",
            severity: ValidationSeverity.Error);
    }

    public static ValidationError AccountNumberTooLong()
    {
        return new ValidationError(
            identifier: AccountIdentifier,
            errorMessage: "Account numbers may only be up to 5 digits long.",
            errorCode: "ACC-NUMBER_TOO_LONG",
            severity: ValidationSeverity.Error);
    }

    public static ValidationError AccountNumberIsEmpty()
    {
        return new ValidationError(
            identifier: AccountIdentifier,
            errorMessage: "Account number is required.",
            errorCode: "ACC-NUMBER_EMPTY",
            severity: ValidationSeverity.Error);
    }

    public static ValidationError AccountNameIsEmpty()
    {
        return new ValidationError(
            identifier: AccountIdentifier,
            errorMessage: "Account name is required.",
            errorCode: "ACC-NAME_EMPTY",
            severity: ValidationSeverity.Error);
    }
}