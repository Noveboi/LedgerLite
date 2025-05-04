using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.Accounting.Core.Domain.Chart;

public sealed class AccountNode : Entity
{
    private AccountNode() {}
    private AccountNode(Guid chartId)
    {
        ChartId = chartId;
    }
    
    public Guid ChartId { get; private init; }
    public Account Account { get; private set; } = null!;
    
    public Guid? ParentId { get; private set; }
    public Account? Parent { get; private set; }

    public static AccountNode CreateRoot(Guid chartId, Account account) => new(chartId)
    {
        Account = account
    };

    public static AccountNode CreateWithParent(Guid chartId, Account account, Account parent) => new(chartId)
    {
        Account = account,
        Parent = parent,
        ParentId = parent.Id
    };
}