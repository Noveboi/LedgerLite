using Ardalis.Result;

namespace LedgerLite.Accounting.Core.Domain.Chart;

internal static class ChartOfAccountsErrors
{
    private const string ChartIdentifier = "ChartOfAccounts";

    public static ValidationError AccountNotRootLevel() => new(
        identifier: ChartIdentifier,
        errorMessage: "Operation is not valid for non-root level accounts.",
        errorCode: "COA-NOT_ROOT",
        severity: ValidationSeverity.Error);
}