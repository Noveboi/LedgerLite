using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;
using LedgerLite.SharedKernel.Domain.Errors;
using LedgerLite.Users.Contracts;

namespace LedgerLite.Accounting.Reporting;

internal sealed class ReportingUserAuthorization(
    IUserRequests user,
    IFiscalPeriodRepository fiscalPeriodRepository)
{
    public async Task<Result<FiscalPeriod>> AuthorizeAsync(Guid userId, Guid fiscalPeriodId, CancellationToken ct)
    {
        if (await fiscalPeriodRepository.GetByIdAsync(id: fiscalPeriodId, token: ct) is not { } fiscalPeriod)
            return Result.NotFound(CommonErrors.NotFound<FiscalPeriod>(id: fiscalPeriodId));

        if (!await user.UserBelongsInOrganizationAsync(userId: userId, organizationId: fiscalPeriod.OrganizationId,
                token: ct))
            return Result.Unauthorized();

        return Result.Success(value: fiscalPeriod);
    }
}