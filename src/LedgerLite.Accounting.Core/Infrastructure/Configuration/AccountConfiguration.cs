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
            .HasMaxLength(6)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(150)
            .IsRequired(false);

        builder.HasEnumeration(x => x.Type);
        builder.HasEnumeration(x => x.Currency);
    }
}