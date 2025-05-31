using Ardalis.Result;

namespace LedgerLite.Accounting.Core.Domain.Accounts;

internal static class AccountErrors
{
    public const string AccountIdentifier = "Account";

    public static ValidationError NoChildrenWhenNotPlaceholder(Account account)
    {
        return new ValidationError(
            AccountIdentifier,
            $"Account '{account}' needs to be a placeholder to have child accounts.",
            "ACC-ADD_NOT_PLACEHOLDER",
            ValidationSeverity.Error);
    }

    public static ValidationError ChildHasDifferentType(AccountType expected, AccountType actual)
    {
        return new ValidationError(
            AccountIdentifier,
            $"Expected child account to be {expected}, got {actual} instead.",
            "ACC-ADD_WRONG_TYPE",
            ValidationSeverity.Error);
    }

    public static ValidationError AddAccountToItself()
    {
        return new ValidationError(
            AccountIdentifier,
            "Cannot add account as a child of itself.",
            "ACC-ADD_TO_SELF",
            ValidationSeverity.Error);
    }

    public static ValidationError AccountNumberTooLong()
    {
        return new ValidationError(
            AccountIdentifier,
            "Account numbers may only be up to 5 digits long.",
            "ACC-NUMBER_TOO_LONG",
            ValidationSeverity.Error);
    }

    public static ValidationError AccountNumberIsEmpty()
    {
        return new ValidationError(
            AccountIdentifier,
            "Account number is required.",
            "ACC-NUMBER_EMPTY",
            ValidationSeverity.Error);
    }

    public static ValidationError AccountNameIsEmpty()
    {
        return new ValidationError(
            AccountIdentifier,
            "Account name is required.",
            "ACC-NAME_EMPTY",
            ValidationSeverity.Error);
    }
}