using Ardalis.Result;
using LedgerLite.SharedKernel.Domain.Errors;
using LedgerLite.Users.Domain;
using LedgerLite.Users.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Users.Application.Users;

internal sealed class UserService(UsersDbContext context) : IUserService
{
    public async Task<Result<User>> GetByIdAsync(Guid id, CancellationToken token)
    {
        return await GetUserAsync(id: id, token: token) is { } user
            ? Result.Success(value: user)
            : Result.NotFound(CommonErrors.NotFound<User>(id: id));
    }

    private Task<User?> GetUserAsync(Guid id, CancellationToken token)
    {
        return context.Users
            .AsSplitQuery()
            .Include(navigationPropertyPath: x => x.OrganizationMember)
            .ThenInclude(navigationPropertyPath: x => x!.Roles)
            .ThenInclude(navigationPropertyPath: x => x.Role)
            .FirstOrDefaultAsync(predicate: x => x.Id == id, cancellationToken: token);
    }
}