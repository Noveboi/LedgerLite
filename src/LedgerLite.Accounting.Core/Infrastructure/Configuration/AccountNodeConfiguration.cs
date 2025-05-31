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
            .HasOne(navigationExpression: x => x.Account)
            .WithOne()
            .HasForeignKey<AccountNode>(foreignKeyExpression: x => x.AccountId);

        builder.Navigation(navigationExpression: x => x.Parent).IsRequired(required: false);
        builder.Navigation(navigationExpression: x => x.Account).IsRequired().AutoInclude();

        builder.HasIndex(indexExpression: x => x.AccountId).IsUnique();
    }
}