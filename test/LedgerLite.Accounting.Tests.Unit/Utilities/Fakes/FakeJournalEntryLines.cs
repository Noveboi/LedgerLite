using Bogus;
using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Tests.Shared;

namespace LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

public sealed class FakeJournalEntryLineOptions
{
    public TransactionType? Type { get; private set; }
    public Account? Account { get; private set; }
    public Guid? AccountId { get; private set; }
    public Guid? EntryId { get; set; }
    public decimal? Amount { get; set; }

    public FakeJournalEntryLineOptions Credit(Account account, decimal amount)
    {
        Account = account;
        AccountId = account.Id;
        Amount = amount;
        Type = TransactionType.Credit;
        return this;
    }

    public FakeJournalEntryLineOptions Debit(Account account, decimal amount)
    {
        Account = account;
        AccountId = account.Id;
        Amount = amount;
        Type = TransactionType.Debit;
        return this;
    }
}

public static class FakeJournalEntryLines
{
    internal static Faker<JournalEntryLine> GetFakerCore(FakeJournalEntryLineOptions options)
    {
        return new PrivateFaker<JournalEntryLine>()
            .UsePrivateConstructor()
            .RuleFor(x => x.TransactionType, options.Type ?? TransactionType.Credit)
            .RuleFor(x => x.Amount, f => options.Amount ?? f.Random.Number(min: 1, max: 1000))
            .RuleFor(x => x.AccountId,
                _ => options.Account?.Id ?? options.AccountId ?? Guid.NewGuid())
            .RuleFor(x => x.Account, _ => options.Account)
            .RuleFor(x => x.EntryId, _ => options.EntryId ?? Guid.NewGuid());
    }

    public static Faker<JournalEntryLine> GetCreditFaker(Action<FakeJournalEntryLineOptions>? configure = null)
    {
        var options = new FakeJournalEntryLineOptions();
        configure?.Invoke(obj: options);

        return GetFakerCore(options: options)
            .RuleFor(x => x.TransactionType, value: TransactionType.Credit);
    }

    public static Faker<JournalEntryLine> GetDebitFaker(Action<FakeJournalEntryLineOptions>? configure = null)
    {
        var options = new FakeJournalEntryLineOptions();
        configure?.Invoke(obj: options);

        return GetFakerCore(options: options)
            .RuleFor(x => x.TransactionType, value: TransactionType.Debit);
    }

    public static List<JournalEntryLine> GenerateStandardLines()
    {
        var credit = GetCreditFaker(o => o.Amount = 10);
        var debit = GetDebitFaker(o => o.Amount = 10);

        return [credit.Generate(), debit.Generate()];
    }

    public static List<JournalEntryLine> Get(params TransactionType[] types)
    {
        var credit = GetCreditFaker();
        var debit = GetDebitFaker();

        return types
            .Select(type => type == TransactionType.Credit
                ? credit.Generate()
                : debit.Generate())
            .ToList();
    }
}