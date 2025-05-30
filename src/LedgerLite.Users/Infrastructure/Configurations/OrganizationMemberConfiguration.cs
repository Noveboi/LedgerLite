using LedgerLite.SharedKernel.Persistence;
using LedgerLite.Users.Domain;
using LedgerLite.Users.Domain.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LedgerLite.Users.Infrastructure.Configurations;

internal sealed class OrganizationMemberConfiguration : IEntityTypeConfiguration<OrganizationMember>
{
    public void Configure(EntityTypeBuilder<OrganizationMember> builder)
    {
        builder.IsDomainEntity();
        builder
            .HasOne(x => x.User)
            .WithOne(x => x.OrganizationMember)
            .IsRequired(false)
            .HasForeignKey<User>(x => x.OrganizationMemberId);
    }
}