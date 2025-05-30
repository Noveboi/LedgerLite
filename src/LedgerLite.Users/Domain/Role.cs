using Microsoft.AspNetCore.Identity;

namespace LedgerLite.Users.Domain;

public sealed class Role : IdentityRole<Guid>
{
    private Role() { }

    public Role(string name, string description = "")
    {
        Name = name;
        Description = description;
    }
    
    public IReadOnlyCollection<UserRole> UserRoles { get; init; } = null!;
    public string? Description { get; init; }
}