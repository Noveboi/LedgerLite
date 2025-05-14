namespace LedgerLite.Users.Contracts.Models;

public sealed record UserDto(
    Guid Id,
    Guid? OrganizationId,
    string Email,
    string Username,
    string FullName);