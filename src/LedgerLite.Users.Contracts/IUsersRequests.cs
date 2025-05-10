using Ardalis.Result;
using LedgerLite.Users.Contracts.Models;

namespace LedgerLite.Users.Contracts;

/// <summary>
/// Cross-module use cases for Users. 
/// </summary>
public interface IUsersRequests
{
    Task<Result<UserDto>> GetUserByIdAsync(Guid id, CancellationToken token);
}