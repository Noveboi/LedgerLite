using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.JournalEntries;

namespace LedgerLite.Accounting.Core.Domain;

/// <summary>
/// The calculated balance of an account using its journal entry lines. 
/// </summary>
public sealed record AccountBalance(Account Account, TransactionType Type, decimal Amount)
{
    public static AccountBalance FromLines(Account account, IEnumerable<JournalEntryLine> lines)
    {
        var lineList = lines.ToList();
        if (lineList.FirstOrDefault(x => x.Account != account) is { } invalidLine)
        {
            throw new ArgumentException($"Lines contains other accounts apart from {account}. ({invalidLine.Account})");
        }

        return Create(account, lineList);
    }

    public static AccountBalance FromGroup(IGrouping<Account, JournalEntryLine> group) => 
        Create(group.Key, group);

    private static AccountBalance Create(Account account, IEnumerable<JournalEntryLine> lines)
    {
        var balance = lines.Aggregate(0m, (current, line) => line.TransactionType switch
        {
            TransactionType.Credit when line.Account.Type.IsCredit() => current + line.Amount,
            TransactionType.Debit when line.Account.Type.IsDebit() => current + line.Amount,
            _ => current - line.Amount,
        });

        var type = balance >= 0
            ? account.Type.IsCredit() ? TransactionType.Credit : TransactionType.Debit
            : account.Type.IsCredit() ? TransactionType.Debit : TransactionType.Credit;
        
        return new AccountBalance(account, type, Math.Abs(balance));
    }
}