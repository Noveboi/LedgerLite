using Ardalis.Result;
using LedgerLite.Accounting.Core.Application.Chart;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;
using LedgerLite.SharedKernel.UseCases;
using LedgerLite.Users.Contracts;

namespace LedgerLite.Accounting.Core.Application.UseCases;

// Removing an account means that JournalEntryLines connect to the account will also be removed. Since
// JournalEntryLines are removed, this means that JournalEntries will also need to be removed.
//
// Generally speaking, this use case is very delicate and needs to be handled with care.

internal sealed record RemoveAccountRequest(Guid UserId, Guid AccountId);
internal sealed class RemoveAccountUseCase(
    IJournalEntryLineRepository journalEntryLineRepository,
    IChartOfAccountsService chartService, 
    IUserRequests userRequests) : IApplicationUseCase<RemoveAccountRequest, Account>
{
    public async Task<Result<Account>> HandleAsync(RemoveAccountRequest request, CancellationToken token) =>
        await userRequests.GetUserByIdAsync(request.UserId, token)
            .BindAsync(user => chartService
                .GetByUserIdAsync(user.Id, token)
                .MapAsync(chart => new { Chart = chart, User = user }))
            .BindAsync(state => state.Chart.Nodes.FirstOrDefault(x => x.Account.Id == request.AccountId) is { } node 
                ? Result.Success(new { state.Chart, state.User, Node = node  }) 
                : Result.Unauthorized())
            .BindAsync(state => state.Node.Children.Count == 0 
                ? Result.Success(state)
                : Result.Invalid(ChartOfAccountsErrors.CannotRemoveAccountWithChildren(state.Node)))
            .BindAsync(async state => await journalEntryLineRepository.GetLinesForAccountAsync(state.Node.Account) is [] 
                ? Result.Success(state)
                : Result.Invalid(ChartOfAccountsErrors.CannotRemoveAccountWithExistingLines(state.Node.Account)))
            .MapAsync(state => state.Node.Account);
}