using Ardalis.Result;
using LedgerLite.Users.Domain.Organizations;

namespace LedgerLite.Users.Infrastructure.Repositories;

internal sealed class OrganizationRepository(UsersDbContext context) : IOrganizationRepository
{
    public Result<Organization> Add(Organization organization)
    {
        return context.Organizations.Add(organization).Entity;
    }
}