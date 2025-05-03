using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Infrastructure.Configuration;
using LedgerLite.SharedKernel.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Accounting.Core.Infrastructure;

internal sealed class AccountingDbContext(DbContextOptions<AccountingDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ConfigureEnumeration<AccountType>()
            .ConfigureEnumeration<JournalEntryType>()
            .ConfigureEnumeration<JournalEntryStatus>()
            .ConfigureEnumeration<TransactionType>()
            .ConfigureEnumeration<Currency>();
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IAccountingEntityConfigurationMarker).Assembly);
    }
}