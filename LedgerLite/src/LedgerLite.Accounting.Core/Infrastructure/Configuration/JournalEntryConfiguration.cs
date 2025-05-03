using LedgerLite.Accounting.Core.Domain.JournalEntries;
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
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasMaxLength(200);
        
        builder.HasEnumeration(x => x.Type);
        builder.HasEnumeration(x => x.Status);
    }
}