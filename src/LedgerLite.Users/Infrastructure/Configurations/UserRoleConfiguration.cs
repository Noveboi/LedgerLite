using LedgerLite.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LedgerLite.Users.Infrastructure.Configurations;

internal sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder
            .HasOne(navigationExpression: x => x.OrganizationMember)
            .WithMany(navigationExpression: x => x.Roles)
            .HasForeignKey(foreignKeyExpression: x => x.OrganizationMemberId);

        builder
            .HasOne(navigationExpression: x => x.Role)
            .WithMany(navigationExpression: x => x.UserRoles)
            .HasForeignKey(foreignKeyExpression: x => x.RoleId);
    }
}