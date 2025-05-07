using Ardalis.Result;
using LedgerLite.Users.Domain.Organizations;

namespace LedgerLite.Users.Infrastructure.Repositories;

public interface IOrganizationRepository
{
    Result<Organization> Add(Organization organization);
}