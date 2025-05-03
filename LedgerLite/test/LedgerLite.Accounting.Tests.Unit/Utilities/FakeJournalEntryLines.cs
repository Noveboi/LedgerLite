using Bogus;
using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Tests.Shared;

namespace LedgerLite.Accounting.Tests.Unit.Utilities;

public sealed class JournalEntryLineFakerOptions
{
    public Guid? AccountId { get; set; }
    public Guid? EntryId { get; set; }
    public decimal? Amount { get; set; }
}
public static class FakeJournalEntryLines
{
    private static Faker<JournalEntryLine> GetFakerCore(JournalEntryLineFakerOptions? options) =>
        new PrivateFaker<JournalEntryLine>()
            .UsePrivateConstructor()
            .RuleFor(x => x.Amount, f => options?.Amount ?? f.Random.Number(1, 1000))
            .RuleFor(x => x.AccountId, _ => options?.AccountId ?? Guid.NewGuid())
            .RuleFor(x => x.EntryId, _ => options?.EntryId ?? Guid.NewGuid());

    public static Faker<JournalEntryLine> GetCreditFaker(Action<JournalEntryLineFakerOptions>? configure = null)
    {
        var options = new JournalEntryLineFakerOptions();
        configure?.Invoke(options);
        
        return GetFakerCore(options)
            .RuleFor(x => x.TransactionType, TransactionType.Credit);
    }

    public static Faker<JournalEntryLine> GetDebitFaker(Action<JournalEntryLineFakerOptions>? configure = null)
    {
        var options = new JournalEntryLineFakerOptions();
        configure?.Invoke(options);
        
        return GetFakerCore(options)
            .RuleFor(x => x.TransactionType, TransactionType.Debit);
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