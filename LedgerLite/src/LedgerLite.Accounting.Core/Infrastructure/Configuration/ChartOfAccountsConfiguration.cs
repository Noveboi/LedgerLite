using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.SharedKernel.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LedgerLite.Accounting.Core.Infrastructure.Configuration;

internal sealed class ChartOfAccountsConfiguration : IEntityTypeConfiguration<ChartOfAccounts>
{
    public void Configure(EntityTypeBuilder<ChartOfAccounts> builder)
    {
        builder.IsDomainEntity();
        builder
            .HasMany(x => x.Accounts)
            .WithOne()
            .HasForeignKey(x => x.ChartId);
    }
}