using Ardalis.Result;
using LedgerLite.Accounting.Core.Application.Chart;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;
using LedgerLite.SharedKernel.Domain.Errors;
using LedgerLite.SharedKernel.UseCases;

namespace LedgerLite.Accounting.Core.Application.UseCases;

internal sealed record GetDetailedAccountRequest(
    Guid UserId, 
    Guid AccountId,
    Guid? FiscalPeriodId);

internal sealed class GetDetailedAccountUseCase(
    IChartOfAccountsService chartService,
    IJournalEntryLineRepository lineRepository)
    : IApplicationUseCase<GetDetailedAccountRequest, AccountWithDetails>
{
    public async Task<Result<AccountWithDetails>> HandleAsync(GetDetailedAccountRequest request, CancellationToken token)
    {
        var chartResult = await chartService.GetByUserIdAsync(request.UserId, token);
        if (!chartResult.IsSuccess)
        {
            return chartResult.Map();
        }

        var chart = chartResult.Value;
        if (chart.Nodes.FirstOrDefault(node => node.Account.Id == request.AccountId) is not { } accountNode)
        {
            return Result.NotFound(CommonErrors.NotFound<Account>(request.AccountId));
        }

        var options = new JournalEntryLineQueryOptions(FiscalPeriodId: request.FiscalPeriodId);
        var lines = await lineRepository.GetLinesForAccountAsync(accountNode.Account, options, token);
        return new AccountWithDetails(
            Node: accountNode,
            Lines: lines);
    }
}