using Ardalis.GuardClauses;
using LedgerLite.SharedKernel;

namespace LedgerLite.Accounting.Domain;

/// <summary>
/// Records a financial transaction 
/// </summary>
public sealed class JournalEntry : Entity
{
    private JournalEntry() { }
    private JournalEntry(
        JournalEntryType type, 
        decimal money, 
        Guid accountId,
        Guid? id = null) : base(id)
    {
        Type = Guard.Against.EnumOutOfRange(type);
        Amount = Guard.Against.NegativeOrZero(money);
        AccountId = accountId;
    }

    public JournalEntryType Type { get; }
    public decimal Amount { get; }
    public Guid AccountId { get; }

    public static JournalEntry Credit(decimal amount, Guid accountId) =>
        new(type: JournalEntryType.Credit,
            money: amount,
            accountId: accountId);
    
    public static JournalEntry Debit(decimal amount, Guid accountId) =>
        new(type: JournalEntryType.Debit,
            money: amount,
            accountId: accountId);
}