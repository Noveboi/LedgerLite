using Ardalis.Result;
using LedgerLite.SharedKernel.Domain.Errors;
using LedgerLite.Users.Contracts;
using LedgerLite.Users.Contracts.Models;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Infrastructure.Repositories;
using LedgerLite.Users.Integrations.Conversions;

namespace LedgerLite.Users.Integrations;

internal sealed class OrganizationRequests(IOrganizationRepository repository) : IOrganizationRequests
{
    public async Task<Result<OrganizationDto>> GetOrganizationByIdAsync(Guid id, CancellationToken token)
    {
        if (await repository.GetByIdAsync(id: id, token: token) is not { } organization)
            return Result.NotFound(CommonErrors.NotFound<Organization>(id: id));

        return organization.ToDto();
    }
}