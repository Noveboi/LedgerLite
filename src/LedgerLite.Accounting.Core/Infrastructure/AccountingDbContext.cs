using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Infrastructure.Configuration;
using LedgerLite.SharedKernel.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LedgerLite.Accounting.Core.Infrastructure;

internal sealed class AccountingDbContext(DbContextOptions<AccountingDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; private set; }
    public DbSet<ChartOfAccounts> Charts { get; private set; }
    public DbSet<JournalEntry> JournalEntries { get; private set; }
    public DbSet<JournalEntryLine> JournalEntryLines { get; private set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Accounting");
        
        modelBuilder
            .ConfigureEnumeration<AccountType>()
            .ConfigureEnumeration<JournalEntryType>()
            .ConfigureEnumeration<JournalEntryStatus>()
            .ConfigureEnumeration<TransactionType>()
            .ConfigureEnumeration<Currency>();
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IAccountingEntityConfigurationMarker).Assembly);
    }
}