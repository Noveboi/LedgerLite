using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.SharedKernel.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LedgerLite.Accounting.Core.Infrastructure.Configuration;

internal sealed class AccountNodeConfiguration : IEntityTypeConfiguration<AccountNode>
{
    public void Configure(EntityTypeBuilder<AccountNode> builder)
    {
        builder.IsDomainEntity();

        builder
            .HasOne(x => x.Account)
            .WithOne()
            .HasForeignKey<AccountNode>(x => x.AccountId);
        
        builder.Navigation(x => x.Parent).IsRequired(false);
        builder.Navigation(x => x.Account).IsRequired().AutoInclude();

        builder.HasIndex(x => x.AccountId).IsUnique();
    }
}