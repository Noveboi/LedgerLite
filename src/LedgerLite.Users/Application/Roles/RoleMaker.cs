using Ardalis.Result;
using LedgerLite.Users.Contracts;
using LedgerLite.Users.Domain;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace LedgerLite.Users.Application.Roles;

internal sealed class RoleMaker(RoleManager<Role> roleManager)
{
    private readonly ILogger _log = Log.ForContext<RoleMaker>();
    private readonly Dictionary<string, bool> _existingRoles = [];
    
    public async Task<Result> CreateApplicationRolesAsync(CancellationToken token)
    {
        _log.Information("Ensuring essential application roles exist.");

        await GetIfRoleExistsAsync(CommonRoles.Owner);
        await GetIfRoleExistsAsync(CommonRoles.Admin);
        await GetIfRoleExistsAsync(CommonRoles.Member);
        await GetIfRoleExistsAsync(CommonRoles.Viewer);
        
        var owner = new Role(CommonRoles.Owner, "The owner of the organization");
        var admin = new Role(CommonRoles.Admin, "Executive rights in the organization");
        var member = new Role(CommonRoles.Member, "Standard organization member");
        var viewer = new Role(CommonRoles.Viewer, "Member with read-only access to the organization");

        return await CreateRoleAsync(owner)
            .BindAsync(_ => CreateRoleAsync(admin))
            .BindAsync(_ => CreateRoleAsync(member))
            .BindAsync(_ => CreateRoleAsync(viewer));
    }

    private async Task GetIfRoleExistsAsync(string name)
    {
        var exists = await roleManager.RoleExistsAsync(name);
        _existingRoles[name] = exists;
    }
    
    private async Task<Result> CreateRoleAsync(Role role)
    {
        if (string.IsNullOrWhiteSpace(role.Name) || _existingRoles[role.Name])
        {
            return Result.NoContent();
        }
        
        _log.Information("Adding role '{name}'", role.Name);
        var result = await roleManager.CreateAsync(role);
        return result.Succeeded
            ? Result.Success()
            : Result.Invalid(result.Errors.Select(x => new ValidationError(
                identifier: x.Code, 
                errorMessage: x.Description)));
    }
}