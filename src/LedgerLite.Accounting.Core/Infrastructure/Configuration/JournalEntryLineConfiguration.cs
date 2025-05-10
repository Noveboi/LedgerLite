using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.SharedKernel.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LedgerLite.Accounting.Core.Infrastructure.Configuration;

internal sealed class JournalEntryLineConfiguration : IEntityTypeConfiguration<JournalEntryLine>
{
    public void Configure(EntityTypeBuilder<JournalEntryLine> builder)
    {
        builder.IsDomainEntity();
        builder.HasEnumeration(x => x.TransactionType);

        builder
            .HasOne(x => x.Entry)
            .WithMany(x => x.Lines)
            .HasForeignKey(x => x.EntryId);

        builder
            .HasOne(x => x.Account)
            .WithMany()
            .HasForeignKey(x => x.AccountId);

        builder.Navigation(x => x.Account).IsRequired().AutoInclude();
    }
}