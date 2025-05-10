using LedgerLite.Users.Domain.Organizations;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Users.Infrastructure.Repositories;

internal sealed class OrganizationRepository(UsersDbContext context) : IOrganizationRepository
{
    public Task<bool> NameExistsAsync(string name, CancellationToken token) =>
        context.Organizations.AnyAsync(x => x.Name == name, cancellationToken: token);

    public Task<Organization?> GetByIdAsync(Guid id, CancellationToken token) => 
        context.Organizations.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: token);

    public void Add(Organization organization) => context.Organizations.Add(organization);
}