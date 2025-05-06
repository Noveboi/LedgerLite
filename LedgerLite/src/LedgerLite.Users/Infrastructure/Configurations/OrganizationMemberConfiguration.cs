using LedgerLite.SharedKernel.Persistence;
using LedgerLite.Users.Domain;
using LedgerLite.Users.Domain.Organization;
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
            .WithOne()
            .IsRequired(false)
            .HasForeignKey<User>(x => x.OrganizationMemberId);
        
        builder.HasEnumeration(x => x.Role);
    }
}