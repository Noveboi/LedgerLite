﻿using LedgerLite.SharedKernel.Persistence;
using LedgerLite.Users.Domain;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Users.Infrastructure;

internal sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) 
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Organization> Organizations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Users");

        builder.ConfigureEnumeration<OrganizationMemberRole>();

        builder.ApplyConfigurationsFromAssembly(typeof(IUserEntityConfigurationMarker).Assembly);
    }
}