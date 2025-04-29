using Bogus;
using LedgerLite.Accounting.Domain;
using LedgerLite.Accounting.Domain.JournalEntries;
using LedgerLite.Tests.Shared;

namespace LedgerLite.Accounting.Tests.Unit.Fakes;

public sealed class JournalEntryLineFakerOptions
{
    public Guid? AccountId { get; set; }
    public Guid? EntryId { get; set; }
}
public static class FakeJournalEntryLines
{
    private static Faker<JournalEntryLine> GetFakerCore(JournalEntryLineFakerOptions? options) =>
        new PrivateFaker<JournalEntryLine>()
            .UsePrivateConstructor()
            .RuleFor(x => x.Amount, f => f.Random.Number(1, 1000))
            .RuleFor(x => x.AccountId, _ => options?.AccountId ?? Guid.NewGuid())
            .RuleFor(x => x.EntryId, _ => options?.EntryId ?? Guid.NewGuid());

    public static Faker<JournalEntryLine> GetCreditFaker(JournalEntryLineFakerOptions? options = null) =>
        GetFakerCore(options)
            .RuleFor(x => x.TransactionType, TransactionType.Credit);
    
    public static Faker<JournalEntryLine> GetDebitFaker(JournalEntryLineFakerOptions? options = null) => 
        GetFakerCore(options)
            .RuleFor(x => x.TransactionType, TransactionType.Debit);

    public static List<JournalEntryLine> GenerateStandardLines()
    {
        var credit = GetCreditFaker();
        var debit = GetDebitFaker();

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