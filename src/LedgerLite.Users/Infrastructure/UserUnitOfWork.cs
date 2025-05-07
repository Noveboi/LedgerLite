using System.Diagnostics.CodeAnalysis;
using LedgerLite.SharedKernel.Persistence;
using LedgerLite.Users.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerLite.Users.Infrastructure;

internal sealed class UserUnitOfWork(
    IServiceProvider serviceProvider,
    UsersDbContext context) : UnitOfWork<UsersDbContext>(context), IUserUnitOfWork
{
    [field: AllowNull, MaybeNull]
    public IOrganizationRepository OrganizationRepository => 
        field ?? serviceProvider.GetRequiredService<IOrganizationRepository>();
}