namespace LedgerLite.Users.Contracts.Models;

public sealed record UserDto(
    Guid Id,
    Guid? MemberId,
    OrganizationDto? Organization,
    IEnumerable<RoleDto> OrganizationRoles,
    string Email,
    string Username,
    string FullName);