using Ardalis.Result;

namespace LedgerLite.Accounting.Core.Domain.Accounts.Metadata;

/// <summary>
/// Value object containing information of secondary importance for accounts.
/// </summary>
public sealed record AccountMetadata(ExpenseType ExpenseType)
{
    public static readonly AccountMetadata Default = new(ExpenseType.Direct);

    public Result Verify(AccountType accountType)
    {
        var errors = new List<ValidationError>();
        if (ExpenseType == ExpenseType.Indirect && accountType != AccountType.Expense)
        {
            errors.Add(AccountMetadataErrors.OnlyExpensesCanBeIndirect(accountType));
        }

        return errors.Count == 0
            ? Result.Success()
            : Result.Invalid(errors);
    }
}