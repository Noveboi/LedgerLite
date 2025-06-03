using LedgerLite.Users.Domain.Organizations;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Users.Infrastructure.Repositories;

internal sealed class OrganizationRepository(UsersDbContext context) : IOrganizationRepository
{
    public Task<bool> NameExistsAsync(string name, CancellationToken token)
    {
        return context.Organizations.AnyAsync(x => x.Name == name, cancellationToken: token);
    }

    public Task<Organization?> GetByIdAsync(Guid id, CancellationToken token)
    {
        return context.Organizations
            .AsSplitQuery()
            .Include(x => x.Members)
            .ThenInclude(x => x.Roles)
            .ThenInclude(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: token);
    }

    public async Task<IReadOnlyList<Organization>> GetAllAsync(CancellationToken token)
    {
        return await context.Organizations.ToListAsync(cancellationToken: token);
    }

    public void Add(Organization organization)
    {
        context.Organizations.Add(entity: organization);
    }
}