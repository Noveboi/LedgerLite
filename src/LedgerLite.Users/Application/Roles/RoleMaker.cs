using Ardalis.Result;
using LedgerLite.Users.Contracts;
using LedgerLite.Users.Domain;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace LedgerLite.Users.Application.Roles;

internal sealed class RoleMaker(RoleManager<Role> roleManager)
{
    private readonly Dictionary<string, bool> _existingRoles = [];
    private readonly ILogger _log = Log.ForContext<RoleMaker>();

    public async Task<Result> CreateApplicationRolesAsync(CancellationToken token)
    {
        _log.Information(messageTemplate: "Ensuring essential application roles exist.");

        await GetIfRoleExistsAsync(name: CommonRoles.Owner);
        await GetIfRoleExistsAsync(name: CommonRoles.Admin);
        await GetIfRoleExistsAsync(name: CommonRoles.Member);
        await GetIfRoleExistsAsync(name: CommonRoles.Viewer);

        var owner = new Role(name: CommonRoles.Owner, description: "The owner of the organization");
        var admin = new Role(name: CommonRoles.Admin, description: "Executive rights in the organization");
        var member = new Role(name: CommonRoles.Member, description: "Standard organization member");
        var viewer = new Role(name: CommonRoles.Viewer,
            description: "Member with read-only access to the organization");

        return await CreateRoleAsync(role: owner)
            .BindAsync(_ => CreateRoleAsync(role: admin))
            .BindAsync(_ => CreateRoleAsync(role: member))
            .BindAsync(_ => CreateRoleAsync(role: viewer));
    }

    private async Task GetIfRoleExistsAsync(string name)
    {
        var exists = await roleManager.RoleExistsAsync(roleName: name);
        _existingRoles[key: name] = exists;
    }

    private async Task<Result> CreateRoleAsync(Role role)
    {
        if (string.IsNullOrWhiteSpace(value: role.Name) || _existingRoles[key: role.Name]) return Result.NoContent();

        _log.Information(messageTemplate: "Adding role '{name}'", propertyValue: role.Name);
        var result = await roleManager.CreateAsync(role: role);
        return result.Succeeded
            ? Result.Success()
            : Result.Invalid(result.Errors.Select(x => new ValidationError(
                identifier: x.Code,
                errorMessage: x.Description)));
    }
}