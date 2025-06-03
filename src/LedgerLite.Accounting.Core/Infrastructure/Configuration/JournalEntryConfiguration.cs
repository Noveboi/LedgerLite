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

        builder.Property(x => x.ReferenceNumber)
            .IsRequired()
            .HasMaxLength(maxLength: 100);

        builder.Property(x => x.Description)
            .IsRequired(required: false)
            .HasMaxLength(maxLength: 200);

        builder
            .HasOne<FiscalPeriod>()
            .WithMany()
            .HasForeignKey(x => x.FiscalPeriodId);

        builder.HasEnumeration(x => x.Type);
        builder.HasEnumeration(x => x.Status);
    }
}