using Bogus;
using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Tests.Shared;

namespace LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

public sealed class FakeJournalEntryLineOptions
{
    public TransactionType? Type { get; set; }
    public Account? Account { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? EntryId { get; set; }
    public decimal? Amount { get; set; }

    public FakeJournalEntryLineOptions Credit(Account account, decimal amount)
    {
        Account = account;
        Amount = amount;
        Type = TransactionType.Credit;
        return this;
    }

    public FakeJournalEntryLineOptions Debit(Account account, decimal amount)
    {
        Account = account;
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
            .RuleFor(property: x => x.TransactionType, value: options.Type ?? TransactionType.Credit)
            .RuleFor(property: x => x.Amount, setter: f => options.Amount ?? f.Random.Number(min: 1, max: 1000))
            .RuleFor(property: x => x.AccountId,
                setter: _ => options.Account?.Id ?? options.AccountId ?? Guid.NewGuid())
            .RuleFor(property: x => x.Account, setter: _ => options.Account)
            .RuleFor(property: x => x.EntryId, setter: _ => options.EntryId ?? Guid.NewGuid());
    }

    public static Faker<JournalEntryLine> GetCreditFaker(Action<FakeJournalEntryLineOptions>? configure = null)
    {
        var options = new FakeJournalEntryLineOptions();
        configure?.Invoke(obj: options);

        return GetFakerCore(options: options)
            .RuleFor(property: x => x.TransactionType, value: TransactionType.Credit);
    }

    public static Faker<JournalEntryLine> GetDebitFaker(Action<FakeJournalEntryLineOptions>? configure = null)
    {
        var options = new FakeJournalEntryLineOptions();
        configure?.Invoke(obj: options);

        return GetFakerCore(options: options)
            .RuleFor(property: x => x.TransactionType, value: TransactionType.Debit);
    }

    public static List<JournalEntryLine> GenerateStandardLines()
    {
        var credit = GetCreditFaker(configure: o => o.Amount = 10);
        var debit = GetDebitFaker(configure: o => o.Amount = 10);

        return [credit.Generate(), debit.Generate()];
    }

    public static List<JournalEntryLine> Get(params TransactionType[] types)
    {
        var credit = GetCreditFaker();
        var debit = GetDebitFaker();

        return types
            .Select(selector: type => type == TransactionType.Credit
                ? credit.Generate()
                : debit.Generate())
            .ToList();
    }
}