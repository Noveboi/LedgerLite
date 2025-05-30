using Microsoft.AspNetCore.Identity;

namespace LedgerLite.Users.Domain;

public sealed class Role : IdentityRole<Guid>
{
    public IReadOnlyCollection<UserRole> UserRoles { get; init; } = null!;
    public string? Description { get; init; }
}