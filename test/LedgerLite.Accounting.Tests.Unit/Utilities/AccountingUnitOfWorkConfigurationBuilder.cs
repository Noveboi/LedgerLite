using LedgerLite.Accounting.Core.Infrastructure;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;

namespace LedgerLite.Accounting.Tests.Unit.Utilities;

internal sealed class AccountingUnitOfWorkConfigurationBuilder(IAccountingUnitOfWork unitOfWork)
{
    public AccountingUnitOfWorkConfigurationBuilder MockFiscalPeriodRepository(IFiscalPeriodRepository repo)
    {
        unitOfWork.FiscalPeriodRepository.Returns(repo);
        return this;
    }

    public AccountingUnitOfWorkConfigurationBuilder MockChartOfAccountsRepository(IChartOfAccountsRepository repo)
    {
        unitOfWork.ChartOfAccountsRepository.Returns(repo);
        return this;
    }

    public AccountingUnitOfWorkConfigurationBuilder MockAccountRepository(IAccountRepository repo)
    {
        unitOfWork.AccountRepository.Returns(repo);
        return this;
    }

    public AccountingUnitOfWorkConfigurationBuilder MockJournalEntryRepository(IJournalEntryRepository repo)
    {
        unitOfWork.JournalEntryRepository.Returns(repo);
        return this;
    }
}