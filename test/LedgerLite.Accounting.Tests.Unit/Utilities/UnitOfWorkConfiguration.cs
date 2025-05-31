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

        unitOfWork.SaveChangesAsync(token: Arg.Any<CancellationToken>()).Returns(returnThis: Result.Success());
    }

    public static async Task AssertThatNoActionWasTaken(this IAccountingUnitOfWork unitOfWork)
    {
        await unitOfWork.DidNotReceive().SaveChangesAsync(token: Arg.Any<CancellationToken>());
        unitOfWork.FiscalPeriodRepository.DidNotReceive().Add(period: Arg.Any<FiscalPeriod>());
        unitOfWork.ChartOfAccountsRepository.DidNotReceive().Add(chart: Arg.Any<ChartOfAccounts>());
        unitOfWork.JournalEntryRepository.DidNotReceive().Add(entry: Arg.Any<JournalEntry>());
        unitOfWork.AccountRepository.DidNotReceive().Add(account: Arg.Any<Account>());
    }
}