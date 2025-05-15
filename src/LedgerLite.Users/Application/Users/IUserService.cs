using Ardalis.Result;
using LedgerLite.Users.Domain;

namespace LedgerLite.Users.Application.Users;

internal interface IUserService
{
    Task<Result<User>> GetByIdAsync(Guid id, CancellationToken token);
}