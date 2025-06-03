using System.Text.Json.Serialization;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;

internal record AccountDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Number { get; init; }
    public required string Description { get; init; }
    public required string Type { get; init; }
    public required string Currency { get; init; }
    public required bool IsControl { get; init; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public required string? ExpenseType { get; init; }
}