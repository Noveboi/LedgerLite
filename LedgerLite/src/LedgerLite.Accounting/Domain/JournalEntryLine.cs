using Ardalis.GuardClauses;
using LedgerLite.Accounting.Domain.Extensions;
using LedgerLite.SharedKernel;

namespace LedgerLite.Accounting.Domain;

/// <summary>
/// Records a financial transaction.
/// </summary>
public sealed class JournalEntryLine : Entity
{
    private JournalEntryLine() { }
    
    /// <summary>
    /// The ID of the associated journal entry.
    /// </summary>
    public Guid EntryId { get; private init; }
    /// <summary>
    /// The ID of the associated account.
    /// </summary>
    public Guid AccountId { get; private init; }
    public TransactionType TransactionType { get; private init; }
    public DateTime OccuredAtUtc { get; private init; }
    public Money Money { get; private init; } = null!;
    
    public static JournalEntryLine Create(
        TransactionType type, 
        Money money, 
        Guid accountId, 
        DateTime occuredAtUtc,
        Guid entryId) =>
        new()
        {
            TransactionType = Guard.Against.EnumOutOfRange(type),
            Money = Guard.Against.NegativeOrZero(money),
            AccountId = accountId,
            EntryId = entryId,
            OccuredAtUtc = occuredAtUtc
        };
}