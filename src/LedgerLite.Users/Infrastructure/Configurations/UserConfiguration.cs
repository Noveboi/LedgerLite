﻿using LedgerLite.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LedgerLite.Users.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.FirstName).HasMaxLength(100);
        builder.Property(x => x.LastName).HasMaxLength(100);

        builder
            .HasOne(x => x.OrganizationMember)
            .WithOne(x => x.User)
            .IsRequired(false)
            .HasForeignKey<User>(x => x.OrganizationMemberId);

        builder.Navigation(x => x.OrganizationMember).AutoInclude();
    }
}