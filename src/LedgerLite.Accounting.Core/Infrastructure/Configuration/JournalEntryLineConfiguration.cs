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

        builder
            .HasOne(navigationExpression: x => x.Entry)
            .WithMany(navigationExpression: x => x.Lines)
            .HasForeignKey(foreignKeyExpression: x => x.EntryId);

        builder
            .HasOne(navigationExpression: x => x.Account)
            .WithMany()
            .HasForeignKey(foreignKeyExpression: x => x.AccountId);

        builder.Navigation(navigationExpression: x => x.Account).IsRequired().AutoInclude();
    }
}