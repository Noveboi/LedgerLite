using Ardalis.Result;
using LedgerLite.Users.Application.Organizations.Requests;
using LedgerLite.Users.Domain.Organizations;

namespace LedgerLite.Users.Application.Organizations;

public interface IOrganizationService
{
    Task<Result<Organization>> CreateAsync(CreateOrganizationRequest request, CancellationToken token);
    Task<Result<Organization>> RemoveAsync(RemoveOrganizationRequest request, CancellationToken token);
}