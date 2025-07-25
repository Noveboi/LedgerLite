﻿using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Tests.Unit.Utilities;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

namespace LedgerLite.Accounting.Tests.Unit.Domain.JournalEntries;

public class JournalEntryBalanceTests
{
    [Fact]
    public void Balanced_WhenCreditEqualsDebit_TwoLines()
    {
        var credit = FakeJournalEntryLines.GetCreditFaker(o => o.Amount = 100).Generate();
        var debit = FakeJournalEntryLines.GetDebitFaker(o => o.Amount = 100).Generate();

        var entry = JournalEntryHelper.CreateWithLines(type: JournalEntryType.Standard, [credit, debit]);

        entry.IsBalanced().ShouldBeTrue();
    }

    [Fact]
    public void Balanced_WhenCreditEqualsDebit_Compound()
    {
        var credits = FakeJournalEntryLines.GetCreditFaker(o => o.Amount = 100).Generate(count: 3);
        var debits = FakeJournalEntryLines.GetDebitFaker(o => o.Amount = 100).Generate(count: 3);

        var entry = JournalEntryHelper.CreateWithLines(type: JournalEntryType.Compound, [..credits, ..debits]);

        entry.IsBalanced().ShouldBeTrue();
    }

    [Fact]
    public void Imbalanced_WhenCreditDoesNotEqualDebit_TwoLines()
    {
        var credit = FakeJournalEntryLines.GetCreditFaker(o => o.Amount = 100).Generate();
        var debit = FakeJournalEntryLines.GetDebitFaker(o => o.Amount = 99.9m).Generate();

        var entry = JournalEntryHelper.CreateWithLines(type: JournalEntryType.Standard, [credit, debit]);

        entry.IsBalanced().ShouldBeFalse();
    }

    [Fact]
    public void Imbalanced_WhenCreditDoesNotEqualDebit_Compound()
    {
        var credits = FakeJournalEntryLines.GetCreditFaker(o => o.Amount = 100).Generate(count: 2);
        var debits = FakeJournalEntryLines.GetDebitFaker(o => o.Amount = 100).Generate(count: 3);

        var entry = JournalEntryHelper.CreateWithLines(type: JournalEntryType.Standard, [..credits, ..debits]);

        entry.IsBalanced().ShouldBeFalse();
    }
}