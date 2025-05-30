﻿using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Core.Endpoints.JournalEntries.Dto;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;

internal static class AccountDtoConversions
{
    public static AccountDto ToDto(this Account account) => new()
    {
        Id = account.Id,
        Name = account.Name,
        Number = account.Number,
        Type = account.Type.ToString(),
        Currency = account.Currency.ToString(),
        IsControl = account.IsPlaceholder,
        Description = account.Description
    };
    
    public static AccountWithLinesDto ToDto(this AccountWithDetails accountWithDetails)
    {
        var account = accountWithDetails.Node.Account; 
            
        return new AccountWithLinesDto
        {
            Id = account.Id,
            Name = account.Name,
            Number = account.Number,
            Type = account.Type.ToString(),
            Currency = account.Currency.ToString(),
            IsControl = account.IsPlaceholder,
            Description = account.Description,
            Lines = accountWithDetails.Lines.Select(JournalEntryLineDto.FromEntity),
        };
    }
}