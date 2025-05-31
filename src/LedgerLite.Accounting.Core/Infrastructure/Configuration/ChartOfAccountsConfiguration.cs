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
        builder.Ignore(propertyExpression: x => x.Accounts);

        builder.Property(propertyExpression: x => x.OrganizationId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.HasIndex(indexExpression: x => x.OrganizationId).IsUnique();

        builder
            .HasMany(navigationExpression: x => x.Nodes)
            .WithOne()
            .HasForeignKey(foreignKeyExpression: x => x.ChartId);

        builder.Navigation(navigationExpression: x => x.Nodes).AutoInclude();
    }
}