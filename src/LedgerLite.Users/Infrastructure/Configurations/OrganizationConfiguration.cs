using LedgerLite.SharedKernel.Persistence;
using LedgerLite.Users.Domain.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LedgerLite.Users.Infrastructure.Configurations;

internal sealed class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.IsDomainEntity();

        builder.Property(propertyExpression: x => x.Name).HasMaxLength(maxLength: 256);
        builder.HasIndex(indexExpression: x => x.Name).IsUnique();

        builder
            .HasMany(navigationExpression: x => x.Members)
            .WithOne()
            .HasForeignKey(foreignKeyExpression: x => x.OrganizationId);

        builder.Navigation(navigationExpression: x => x.Members).AutoInclude();
    }
}