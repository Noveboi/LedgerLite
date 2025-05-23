using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.Accounts;

namespace LedgerLite.Accounting.Reporting;

public static class AccountingMath
{
    public static decimal Credit(Account account, decimal runningBalance, decimal amount)
    {
        if (account.Type.IsCredit())
            return runningBalance + amount;

        return runningBalance - amount;
    }

    public static decimal Debit(Account account, decimal runningBalance, decimal amount)
    {
        if (account.Type.IsDebit())
            return runningBalance + amount;
        
        return runningBalance - amount;
    }

    public static decimal CreditOrDebit(
        TransactionType type,
        Account account,
        decimal runningBalance,
        decimal amount)
    {
        if (type == TransactionType.Credit)
            return Credit(account, runningBalance, amount);
        
        return Debit(account, runningBalance, amount);
    }
}