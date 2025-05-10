using LedgerLite.Users.Domain.Organizations;

namespace LedgerLite.Users.Infrastructure.Repositories;

public interface IOrganizationRepository
{
    Task<bool> NameExistsAsync(string name, CancellationToken token);
    Task<Organization?> GetByIdAsync(Guid id, CancellationToken token);
    void Add(Organization organization);
}