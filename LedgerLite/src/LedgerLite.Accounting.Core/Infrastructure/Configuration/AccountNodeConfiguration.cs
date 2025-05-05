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
        
        builder.Navigation(x => x.Parent).IsRequired(false);
        builder.Navigation(x => x.Account).AutoInclude();
    }
}