using System.Diagnostics.CodeAnalysis;
using LedgerLite.SharedKernel.Persistence;
using LedgerLite.Users.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerLite.Users.Infrastructure;

internal sealed class UsersUnitOfWork(
    IServiceProvider serviceProvider,
    UsersDbContext context) : UnitOfWork<UsersDbContext>(context), IUsersUnitOfWork
{
    [field: AllowNull]
    [field: MaybeNull]
    public IOrganizationRepository OrganizationRepository =>
        field ?? serviceProvider.GetRequiredService<IOrganizationRepository>();
}