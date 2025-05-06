using LedgerLite.Users.Domain;
using LedgerLite.Users.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Users.Infrastructure;

internal sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) 
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Users");

        builder.ApplyConfigurationsFromAssembly(typeof(IUserEntityConfigurationMarker).Assembly);
    }
}