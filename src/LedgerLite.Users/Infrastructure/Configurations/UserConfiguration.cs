using LedgerLite.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LedgerLite.Users.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(propertyExpression: x => x.FirstName).HasMaxLength(maxLength: 100);
        builder.Property(propertyExpression: x => x.LastName).HasMaxLength(maxLength: 100);

        builder
            .HasOne(navigationExpression: x => x.OrganizationMember)
            .WithOne(navigationExpression: x => x.User)
            .IsRequired(required: false)
            .HasForeignKey<User>(foreignKeyExpression: x => x.OrganizationMemberId);

        builder.Navigation(navigationExpression: x => x.OrganizationMember).AutoInclude();
    }
}