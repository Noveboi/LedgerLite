using Ardalis.Result;

namespace LedgerLite.Accounting.Core.Domain.Accounts.Metadata;

internal static class AccountMetadataErrors
{
    public static ValidationError OnlyExpensesCanBeIndirect(AccountType accountType) => new(
        identifier: AccountErrors.AccountIdentifier,
        $"Only {AccountType.Expense} accounts can be {nameof(ExpenseType.Indirect)}. " +
                      $"You can provided {accountType}",
        errorCode: "ACCMETA-INDIRECT",
        severity: ValidationSeverity.Error);
}