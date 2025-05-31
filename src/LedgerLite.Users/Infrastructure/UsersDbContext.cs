using LedgerLite.Users.Domain;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Users.Infrastructure;

internal sealed class UsersDbContext(DbContextOptions<UsersDbContext> options)
    : IdentityDbContext<
        User,
        Role,
        Guid,
        IdentityUserClaim<Guid>,
        UserRole,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>(options)
{
    public DbSet<Organization> Organizations { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Users");

        builder.ApplyConfigurationsFromAssembly(typeof(IUserEntityConfigurationMarker).Assembly);
    }
}