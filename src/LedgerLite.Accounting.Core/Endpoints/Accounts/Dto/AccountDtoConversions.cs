using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Accounts.Metadata;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Core.Endpoints.JournalEntries.Dto;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;

internal static class AccountDtoConversions
{
    public static AccountDto ToDto(this Account account)
    {
        return new AccountDto
        {
            Id = account.Id,
            Name = account.Name,
            Number = account.Number,
            Type = account.Type.ToString(),
            Currency = account.Currency.ToString(),
            IsControl = account.IsPlaceholder,
            Description = account.Description,
            ExpenseType = account.Metadata.ExpenseType switch
            {
                ExpenseType.Undefined => null,
                var type => type.ToString(),
            }
        };
    }

    public static AccountWithLinesDto ToDto(this AccountWithDetails accountWithDetails)
    {
        var account = accountWithDetails.Node.Account;
        return new AccountWithLinesDto
        {
            Account = account.ToDto(),
            Lines = accountWithDetails.Lines.Select(selector: JournalEntryLineDto.FromEntity)
        };
    }
}