using Ardalis.Result;
using LedgerLite.Users.Domain;
using Microsoft.AspNetCore.Identity;

namespace LedgerLite.Users.Application;

internal sealed class RoleMaker(RoleManager<Role> roleManager)
{
    public async Task<Result> CreateApplicationRolesAsync()
    {
        var owner = new Role("Owner", "The owner of the organization");
        var admin = new Role("Admin", "Executive rights in the organization");
        var member = new Role("Member", "Standard organization member");
        var viewer = new Role("Viewer", "Member with read-only access to the organization");

        return await CreateRoleAsync(owner)
            .BindAsync(_ => CreateRoleAsync(admin))
            .BindAsync(_ => CreateRoleAsync(member))
            .BindAsync(_ => CreateRoleAsync(viewer));
    }

    private async Task<Result> CreateRoleAsync(Role role)
    {
        var result = await roleManager.CreateAsync(role);
        return result.Succeeded
            ? Result.Success()
            : Result.Invalid(result.Errors.Select(x => new ValidationError(
                identifier: x.Code, 
                errorMessage: x.Description)));
    }
}