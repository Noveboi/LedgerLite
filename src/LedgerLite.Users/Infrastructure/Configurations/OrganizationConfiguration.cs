﻿using LedgerLite.SharedKernel.Persistence;
using LedgerLite.Users.Domain.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LedgerLite.Users.Infrastructure.Configurations;

internal sealed class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.IsDomainEntity();

        builder.Property(x => x.Name).HasMaxLength(maxLength: 256);
        builder.HasIndex(x => x.Name).IsUnique();

        builder
            .HasMany(x => x.Members)
            .WithOne(x => x.Organization)
            .HasForeignKey(x => x.OrganizationId);

        builder.Navigation(x => x.Members).AutoInclude();
    }
}