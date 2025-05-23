﻿using LedgerLite.Accounting.Core.Domain.Accounts;
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
    public TransactionType TransactionType { get; private init; } = null!;
    public decimal Amount { get; private init; } 
    
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

    public override string ToString() => $"{TransactionType.Name} '{Account.Name}' - {Amount:N}";
}