using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.Accounting.Core.Infrastructure;

namespace LedgerLite.Accounting.Tests.Unit.Utilities;

internal static class AccountingUnitOfWorkConfiguration
{
    public static void ConfigureForTests(
        this IAccountingUnitOfWork unitOfWork,
        Action<AccountingUnitOfWorkConfigurationBuilder>? configure = null)
    {
        var builder = new AccountingUnitOfWorkConfigurationBuilder(unitOfWork: unitOfWork);
        configure?.Invoke(obj: builder);

        unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Result.Success());
    }

    public static async Task AssertThatNoActionWasTaken(this IAccountingUnitOfWork unitOfWork)
    {
        await unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        unitOfWork.FiscalPeriodRepository.DidNotReceive().Add(Arg.Any<FiscalPeriod>());
        unitOfWork.ChartOfAccountsRepository.DidNotReceive().Add(Arg.Any<ChartOfAccounts>());
        unitOfWork.JournalEntryRepository.DidNotReceive().Add(Arg.Any<JournalEntry>());
        unitOfWork.AccountRepository.DidNotReceive().Add(Arg.Any<Account>());
    }
}