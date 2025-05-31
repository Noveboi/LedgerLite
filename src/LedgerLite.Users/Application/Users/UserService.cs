using Ardalis.Result;
using LedgerLite.SharedKernel.Domain.Errors;
using LedgerLite.Users.Domain;
using LedgerLite.Users.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Users.Application.Users;

internal sealed class UserService(UsersDbContext context) : IUserService
{
    public async Task<Result<User>> GetByIdAsync(Guid id, CancellationToken token) =>
        await GetUserAsync(id, token) is { } user
            ? Result.Success(user)
            : Result.NotFound(CommonErrors.NotFound<User>(id));

    private Task<User?> GetUserAsync(Guid id, CancellationToken token) =>
        context.Users
            .AsSplitQuery()
            .Include(x => x.OrganizationMember)
            .ThenInclude(x => x!.Roles)
            .ThenInclude(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: token);
}