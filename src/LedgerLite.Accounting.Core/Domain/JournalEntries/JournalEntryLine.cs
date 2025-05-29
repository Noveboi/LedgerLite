using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.Accounting.Core.Domain.JournalEntries;

/// <summary>
/// Records one part of a financial transaction.
/// </summary>
public sealed class JournalEntryLine : AuditableEntity
{
    private JournalEntryLine() { }
    
    /// <summary>
    /// The ID of the associated journal entry.
    /// </summary>
    public Guid EntryId { get; private init; }

    public JournalEntry Entry { get; private init; } = null!;
    public Guid AccountId { get; private init; }
    public Account Account { get; private init; } = null!;
    public TransactionType TransactionType { get; private init; }
    public decimal Amount { get; private init; }

    public Account GetTransferAccount()
    {
        if (Entry is null)
            throw new InvalidOperationException("Related JournalEntry is null.");

        if (Entry.Type == JournalEntryType.Compound)
            throw new NotSupportedException($"{nameof(GetTransferAccount)} is not supported for compound entries.");

        var otherLine = Entry.Lines.FirstOrDefault(x => x.Id != Id);
        if (otherLine is null)
            throw new InvalidOperationException("Second entry line does not exist.");

        if (otherLine.Account is null)
            throw new InvalidOperationException("Account navigation for other entry line is null");

        return otherLine.Account;
    }
    
    public static JournalEntryLine Create(
        TransactionType type, 
        decimal amount, 
        Guid accountId, 
        Guid entryId) =>
        new()
        {
            TransactionType = type,
            Amount = amount,
            AccountId = accountId,
            EntryId = entryId,
        };

    public override string ToString() => $"{TransactionType.ToString()} '{Account.Name}' - {Amount:N}";
}