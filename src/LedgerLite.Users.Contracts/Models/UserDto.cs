namespace LedgerLite.Users.Contracts.Models;

public sealed record UserDto(
    Guid Id,
    Guid? OrganizationId,
    string Username,
    string FullName);