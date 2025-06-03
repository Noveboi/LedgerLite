using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Accounts;

namespace LedgerLite.Accounting.Core.Domain.Chart;

internal static class ChartOfAccountsErrors
{
    private const string ChartIdentifier = "ChartOfAccounts";

    public static ValidationError AccountNotRootLevel()
    {
        return new ValidationError(
            identifier: ChartIdentifier,
            errorMessage: "Operation is not valid for non-root level accounts.",
            errorCode: "COA-NOT_ROOT",
            severity: ValidationSeverity.Error);
    }

    public static ValidationError AccountAlreadyExists(Account existingAccount)
    {
        return new ValidationError(
            identifier: ChartIdentifier,
            $"Account '{existingAccount}' already exists.",
            errorCode: "COA-EXISTS",
            severity: ValidationSeverity.Error);
    }

    public static ValidationError AccountNotFound(Guid id)
    {
        return new ValidationError(
            identifier: ChartIdentifier,
            $"Account with ID '{id}' does not exist in chart.",
            errorCode: "COA-ACCOUNT_NOT_FOUND",
            severity: ValidationSeverity.Error);
    }

    public static ValidationError AccountNotChild(Account parent, Account child)
    {
        return new ValidationError(
            identifier: ChartIdentifier,
            $"Account '{child}' is not a child of '{parent}'",
            errorCode: "COA-CHILD_NOT_FOUND",
            severity: ValidationSeverity.Error);
    }

    public static ValidationError AccountHasNoChildrenToRemove(Account account)
    {
        return new ValidationError(
            identifier: ChartIdentifier,
            $"Account '{account}' does not have children.",
            errorCode: "COA-NO_CHILDREN_TO_REMOVE",
            severity: ValidationSeverity.Error);
    }

    public static ValidationError MoveToSameParent()
    {
        return new ValidationError(
            identifier: ChartIdentifier,
            errorMessage: "Moving to the same parent is not allowed.",
            errorCode: "COA-MOVE_SAME_PARENT",
            severity: ValidationSeverity.Error);
    }

    public static ValidationError CannotRemoveAccountWithChildren(AccountNode node)
    {
        return new ValidationError(
            identifier: ChartIdentifier,
            errorMessage: $"Cannot remove account '{node.Account.Name}' with children (found {node.Children.Count} children).",
            errorCode: "COA-REMOVE_NO_CHILDREN",
            severity: ValidationSeverity.Error);
    }

    public static ValidationError CannotRemoveAccountWithExistingLines(Account account)
    {
        return new ValidationError(
            identifier: ChartIdentifier,
            errorMessage: $"Cannot remove account with existing journal entry lines. Remove all entries associated with " +
                $"'{account.Name}' and try again.",
            errorCode: "COA-REMOVE_NO_LINES",
            severity: ValidationSeverity.Error);
    }

    public static ValidationError AccountInvalidExpenseType(Account parent, Account target)
    {
        return new ValidationError(
            identifier: ChartIdentifier,
            errorMessage: $"'{target}' has different expense type ({target.Metadata.ExpenseType}) from parent '{parent}' " +
                          $"({parent.Metadata.ExpenseType})",
            errorCode: "COA-EXPENSE_TYPE_DIFF",
            severity: ValidationSeverity.Error);
    }
}