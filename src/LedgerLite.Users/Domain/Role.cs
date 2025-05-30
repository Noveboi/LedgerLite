using Microsoft.AspNetCore.Identity;

namespace LedgerLite.Users.Domain;

public sealed class Role : IdentityRole<Guid>
{
    public IReadOnlyCollection<UserRole> UserRoles { get; set; } = null!;
    public string? Description { get; set; } 
}