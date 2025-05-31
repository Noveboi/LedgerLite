using Ardalis.Result;
using LedgerLite.Users.Domain;
using Microsoft.AspNetCore.Identity;

namespace LedgerLite.Users.Application.Roles;

internal sealed class RoleService(IRoleStore<Role> store, RoleManager<Role> manager) : IRoleService
{
    public async Task<Result<Role>> GetByNameAsync(string name, CancellationToken ct)
    {
        var normalizedName = manager.NormalizeKey(name);

        if (await store.FindByNameAsync(normalizedName, ct) is not { } role)
            return Result.NotFound($"Role '{normalizedName}' does not exist.");

        return role;
    }
}