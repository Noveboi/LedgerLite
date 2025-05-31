using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Accounts;

namespace LedgerLite.Accounting.Core.Domain.Chart;

internal static class ChartOfAccountsErrors
{
    private const string ChartIdentifier = "ChartOfAccounts";

    public static ValidationError AccountNotRootLevel()
    {
        return new ValidationError(
            ChartIdentifier,
            "Operation is not valid for non-root level accounts.",
            "COA-NOT_ROOT",
            ValidationSeverity.Error);
    }

    public static ValidationError AccountAlreadyExists(Account existingAccount)
    {
        return new ValidationError(
            ChartIdentifier,
            $"Account '{existingAccount}' already exists.",
            "COA-EXISTS",
            ValidationSeverity.Error);
    }

    public static ValidationError AccountNotFound(Guid id)
    {
        return new ValidationError(
            ChartIdentifier,
            $"Account with ID '{id}' does not exist in chart.",
            "COA-ACCOUNT_NOT_FOUND",
            ValidationSeverity.Error);
    }

    public static ValidationError AccountNotChild(Account parent, Account child)
    {
        return new ValidationError(
            ChartIdentifier,
            $"Account '{child}' is not a child of '{parent}'",
            "COA-CHILD_NOT_FOUND",
            ValidationSeverity.Error);
    }

    public static ValidationError AccountHasNoChildrenToRemove(Account account)
    {
        return new ValidationError(
            ChartIdentifier,
            $"Account '{account}' does not have children.",
            "COA-NO_CHILDREN_TO_REMOVE",
            ValidationSeverity.Error);
    }

    public static ValidationError MoveToSameParent()
    {
        return new ValidationError(
            ChartIdentifier,
            "Moving to the same parent is not allowed.",
            "COA-MOVE_SAME_PARENT",
            ValidationSeverity.Error);
    }

    public static ValidationError CannotRemoveAccountWithChildren(AccountNode node)
    {
        return new ValidationError(
            ChartIdentifier,
            $"Cannot remove account '{node.Account.Name}' with children (found {node.Children.Count} children).",
            "COA-REMOVE_NO_CHILDREN",
            ValidationSeverity.Error);
    }

    public static ValidationError CannotRemoveAccountWithExistingLines(Account account)
    {
        return new ValidationError(
            ChartIdentifier,
            $"Cannot remove account with existing journal entry lines. Remove all entries associated with " +
            $"'{account.Name}' and try again.",
            "COA-REMOVE_NO_LINES",
            ValidationSeverity.Error);
    }
}