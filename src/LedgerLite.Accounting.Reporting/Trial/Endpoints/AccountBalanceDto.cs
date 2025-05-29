using LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;

namespace LedgerLite.Accounting.Reporting.Trial.Endpoints;

internal sealed record AccountBalanceDto(
    SlimAccountDto Account,
    decimal Credit,
    decimal Debit);