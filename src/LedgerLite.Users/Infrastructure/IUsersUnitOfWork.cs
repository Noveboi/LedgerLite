using LedgerLite.SharedKernel.Persistence;
using LedgerLite.Users.Infrastructure.Repositories;

namespace LedgerLite.Users.Infrastructure;

public interface IUsersUnitOfWork : IUnitOfWork
{
    IOrganizationRepository OrganizationRepository { get; }
}