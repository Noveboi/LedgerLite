using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.SharedKernel.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LedgerLite.Accounting.Core.Infrastructure.Configuration;

internal sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.IsDomainEntity();
        builder.Property(x => x.Number)
            .HasMaxLength(maxLength: 6)
            .IsUnicode(unicode: false)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(maxLength: 100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(maxLength: 150)
            .IsRequired(required: false);

        builder.OwnsOne(x => x.Metadata);

        builder.HasEnumeration(x => x.Type);
        builder.HasEnumeration(x => x.Currency);
    }
}