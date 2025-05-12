using Ardalis.Result;
using LedgerLite.Users.Contracts.Models;

namespace LedgerLite.Users.Contracts;

public interface IOrganizationRequests
{
    Task<Result<OrganizationDto>> GetOrganizationByIdAsync(Guid id, CancellationToken token);
}