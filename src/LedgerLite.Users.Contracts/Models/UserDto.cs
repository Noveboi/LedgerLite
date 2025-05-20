namespace LedgerLite.Users.Contracts.Models;

public sealed record UserDto(
    Guid Id,
    OrganizationDto? Organization,
    string Email,
    string Username,
    string FullName);