using Ardalis.Result;
using LedgerLite.Accounting.Core.Application.Chart;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Core.Infrastructure;
using LedgerLite.SharedKernel.UseCases;
using LedgerLite.Users.Contracts;

namespace LedgerLite.Accounting.Core.Application.UseCases;

// Removing an account means that JournalEntryLines connect to the account will also be removed. Since
// JournalEntryLines are removed, this means that JournalEntries will also need to be removed.
//
// Generally speaking, this use case is very delicate and needs to be handled with care.

internal sealed record RemoveAccountRequest(Guid UserId, Guid AccountId);

internal sealed class RemoveAccountUseCase(
    IAccountingUnitOfWork unitOfWork,
    IChartOfAccountsService chartService,
    IUserRequests userRequests) : IApplicationUseCase<RemoveAccountRequest, Account>
{
    public async Task<Result<Account>> HandleAsync(RemoveAccountRequest request, CancellationToken token)
    {
        return await userRequests.GetUserByIdAsync(id: request.UserId, token: token)
            .BindAsync(bindFunc: user => chartService
                .GetByUserIdAsync(userId: user.Id, token: token)
                .MapAsync(func: chart => new { Chart = chart, User = user }))
            .BindAsync(bindFunc: state =>
                state.Chart.Nodes.FirstOrDefault(predicate: x => x.Account.Id == request.AccountId) is { } node
                    ? Result.Success(value: new { state.Chart, state.User, Node = node, node.Account })
                    : Result.Unauthorized())
            .BindAsync(bindFunc: state => state.Node.Children.Count == 0
                ? Result.Success(value: state)
                : Result.Invalid(
                    validationError: ChartOfAccountsErrors.CannotRemoveAccountWithChildren(node: state.Node)))
            .BindAsync(bindFunc: async state =>
                await unitOfWork.JournalEntryLineRepository.GetLinesForAccountAsync(account: state.Account) is []
                    ? Result.Success(value: state)
                    : Result.Invalid(
                        validationError: ChartOfAccountsErrors.CannotRemoveAccountWithExistingLines(
                            account: state.Account)))
            .BindAsync(bindFunc: state =>
            {
                unitOfWork.AccountRepository.Remove(account: state.Account);
                return Result.Success(value: state);
            })
            .BindAsync(bindFunc: state => unitOfWork
                .SaveChangesAsync(token: token)
                .MapAsync(func: () => state))
            .MapAsync(func: state => state.Account);
    }
}