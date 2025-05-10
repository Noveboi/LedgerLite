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
        builder.Ignore(x => x.Accounts);

        builder.Property(x => x.OrganizationId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.HasIndex(x => x.OrganizationId).IsUnique();
        
        builder
            .HasMany(x => x.Nodes)
            .WithOne()
            .HasForeignKey(x => x.ChartId);

        builder.Navigation(x => x.Nodes).AutoInclude();
    }
}