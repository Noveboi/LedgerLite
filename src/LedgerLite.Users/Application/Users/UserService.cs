using Ardalis.Result;
using LedgerLite.SharedKernel.Domain.Errors;
using LedgerLite.Users.Domain;
using Microsoft.AspNetCore.Identity;

namespace LedgerLite.Users.Application.Users;

internal sealed class UserService(UserManager<User> userManager) : IUserService
{
    public async Task<Result<User>> GetByIdAsync(Guid id, CancellationToken token) =>
        await userManager.FindByIdAsync(id.ToString()) is { } user
            ? Result.Success(user)
            : Result.NotFound(CommonErrors.NotFound<User>(id));
}