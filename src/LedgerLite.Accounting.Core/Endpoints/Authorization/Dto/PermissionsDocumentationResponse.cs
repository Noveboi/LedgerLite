namespace LedgerLite.Accounting.Core.Endpoints.Authorization.Dto;

internal sealed record PermissionsDocumentationResponse(
    string Policy,
    IEnumerable<string> AllowedRoles);