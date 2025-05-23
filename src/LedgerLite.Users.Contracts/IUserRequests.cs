using Ardalis.Result;
using LedgerLite.Users.Contracts.Models;

namespace LedgerLite.Users.Contracts;

/// <summary>
/// Cross-module use cases for Users. 
/// </summary>
public interface IUserRequests
{
    Task<Result<UserDto>> GetUserByIdAsync(Guid id, CancellationToken token);
    Task<bool> UserBelongsInOrganizationAsync(Guid userId, Guid organizationId, CancellationToken token);
}