using Ardalis.GuardClauses;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.Accounting.Core.Domain.JournalEntries;

/// <summary>
///     Records one part of a financial transaction.
/// </summary>
public sealed class JournalEntryLine : AuditableEntity
{
    private JournalEntryLine() { }

    /// <summary>
    ///     The ID of the associated journal entry.
    /// </summary>
    public Guid EntryId { get; private init; }

    public JournalEntry Entry { get; private init; } = null!;
    public Guid AccountId { get; private set; }
    public Account Account { get; private init; } = null!;
    public TransactionType TransactionType { get; private set; }
    public decimal Amount { get; private set; }

    public JournalEntryLine? GetRelatedLine()
    {
        if (Entry is null)
            throw new InvalidOperationException(message: "Related JournalEntry is null.");

        if (Entry.Type == JournalEntryType.Compound)
            throw new NotSupportedException(
                $"{nameof(GetTransferAccount)} is not supported for compound entries.");

        return Entry.Lines.FirstOrDefault(x => x.Id != Id);
    }
    
    public Account GetTransferAccount()
    {
        var otherLine = GetRelatedLine() ?? throw new InvalidOperationException("Only one line exists.");

        if (otherLine.Account is null)
            throw new InvalidOperationException(message: "Account navigation for other entry line is null");

        return otherLine.Account;
    }

    public static JournalEntryLine Create(
        TransactionType type,
        decimal amount,
        Guid accountId,
        Guid entryId)
    {
        return new JournalEntryLine
        {
            TransactionType = type,
            Amount = amount,
            AccountId = accountId,
            EntryId = entryId
        };
    }

    public void Update(UpdateLineRequest request)
    {
        var otherLine = GetRelatedLine();

        if (request.Type is { } type && type != TransactionType)
        {
            TransactionType = type;
            if (otherLine != null) otherLine.TransactionType = TransactionType.Opposite();
        }

        if (request.Amount is { } amount && amount != Amount)
        {
            Amount = amount;
            if (otherLine != null) otherLine.Amount = amount;
        }

        if (otherLine != null && request.TransferAccountId is { } transferId && transferId != otherLine.AccountId)
        {
            otherLine.AccountId = transferId;
        }
    }

    public override string ToString()
    {
        return $"{TransactionType.ToString()} '{Account.Name}' - {Amount:N}";
    }
}