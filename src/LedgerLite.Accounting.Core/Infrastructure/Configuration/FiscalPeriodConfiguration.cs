using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.SharedKernel.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LedgerLite.Accounting.Core.Infrastructure.Configuration;

internal sealed class FiscalPeriodConfiguration : IEntityTypeConfiguration<FiscalPeriod>
{
    public void Configure(EntityTypeBuilder<FiscalPeriod> builder)
    {
        builder.IsDomainEntity();
        builder.Property(x => x.Name).HasMaxLength(maxLength: 64);

        builder.HasIndex(x => x.OrganizationId);
    }
}