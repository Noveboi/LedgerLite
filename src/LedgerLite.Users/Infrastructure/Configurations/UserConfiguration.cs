﻿using LedgerLite.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LedgerLite.Users.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.FirstName).HasMaxLength(maxLength: 100);
        builder.Property(x => x.LastName).HasMaxLength(maxLength: 100);

        builder
            .HasOne(x => x.OrganizationMember)
            .WithOne(x => x.User)
            .IsRequired(required: false)
            .HasForeignKey<User>(x => x.OrganizationMemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.OrganizationMember).AutoInclude();
    }
}