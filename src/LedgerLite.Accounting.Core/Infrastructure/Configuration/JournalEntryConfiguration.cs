using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.SharedKernel.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LedgerLite.Accounting.Core.Infrastructure.Configuration;

internal sealed class JournalEntryConfiguration : IEntityTypeConfiguration<JournalEntry>
{
    public void Configure(EntityTypeBuilder<JournalEntry> builder)
    {
        builder.IsDomainEntity();

        builder.Property(propertyExpression: x => x.ReferenceNumber)
            .IsRequired()
            .HasMaxLength(maxLength: 100);

        builder.Property(propertyExpression: x => x.Description)
            .IsRequired(required: false)
            .HasMaxLength(maxLength: 200);

        builder
            .HasOne<FiscalPeriod>()
            .WithMany()
            .HasForeignKey(foreignKeyExpression: x => x.FiscalPeriodId);

        builder.HasEnumeration(propertyExpression: x => x.Type);
        builder.HasEnumeration(propertyExpression: x => x.Status);
    }
}