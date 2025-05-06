using LedgerLite.SharedKernel.Persistence;
using LedgerLite.Users.Domain.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LedgerLite.Users.Infrastructure.Configurations;

internal sealed class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.IsDomainEntity();
        builder.Property(x => x.Name).HasMaxLength(256);

        builder
            .HasMany(x => x.Members)
            .WithOne()
            .HasForeignKey(x => x.OrganizationId);

        builder.Navigation(x => x.Members).AutoInclude();
    }
}