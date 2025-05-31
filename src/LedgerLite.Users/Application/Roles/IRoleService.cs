using Ardalis.Result;
using LedgerLite.Users.Domain;

namespace LedgerLite.Users.Application.Roles;

public interface IRoleService
{
    Task<Result<Role>> GetByNameAsync(string name, CancellationToken ct);
}