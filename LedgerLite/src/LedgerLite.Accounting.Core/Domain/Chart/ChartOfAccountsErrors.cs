using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Accounts;

namespace LedgerLite.Accounting.Core.Domain.Chart;

internal static class ChartOfAccountsErrors
{
    private const string ChartIdentifier = "ChartOfAccounts";

    public static ValidationError AccountNotRootLevel() => new(
        identifier: ChartIdentifier,
        errorMessage: "Operation is not valid for non-root level accounts.",
        errorCode: "COA-NOT_ROOT",
        severity: ValidationSeverity.Error);

    public static ValidationError AccountAlreadyExists(Account existingAccount) => new(
        identifier: ChartIdentifier,
        errorMessage: $"Account {existingAccount} already exists.",
        errorCode: "COA-EXISTS",
        severity: ValidationSeverity.Error);
}