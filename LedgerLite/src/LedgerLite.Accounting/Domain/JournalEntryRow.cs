using Ardalis.GuardClauses;
using LedgerLite.Accounting.Domain.Extensions;
using LedgerLite.SharedKernel;

namespace LedgerLite.Accounting.Domain;

/// <summary>
/// Records a financial transaction.
/// </summary>
public sealed class JournalEntryRow : Entity
{
    private JournalEntryRow() { }
    
    public Guid EntryId { get; private init; }
    public Guid AccountId { get; private init; }
    public TransactionType TransactionType { get; private init; }
    public DateTime OccuredAtUtc { get; private init; }
    public Money Money { get; private init; }
    

    public static JournalEntryRow Create(
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