using Ardalis.Result;
using LedgerLite.Accounting.Core.Infrastructure;
using LedgerLite.SharedKernel.UseCases;
using LedgerLite.Users.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Accounting.Core.Application.UseCases;

internal sealed record DoesPeriodBelongToUser(Guid UserId, Guid PeriodId);
internal sealed class PeriodBelongsToUser(
    IUserRequests userRequests,
    AccountingDbContext context) : IApplicationUseCase<DoesPeriodBelongToUser>
{
    public async Task<Result> HandleAsync(DoesPeriodBelongToUser request, CancellationToken token)
    {
        var userResult = await userRequests.GetUserByIdAsync(id: request.UserId, token: token);
        if (userResult.Value is not { } user) return userResult.Map();
        
        if (user.Organization is not { } organization) 
            return Result.Invalid(new ValidationError("User is not in an organization."));

        return await context.FiscalPeriods.AnyAsync(x => x.OrganizationId == organization.Id && x.Id == request.PeriodId, token)
            ? Result.Success()
            : Result.Unauthorized();
    }
}