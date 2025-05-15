namespace LedgerLite.Users.Application.Organizations.Requests;

public sealed record CreateOrganizationRequest(Guid UserId, string Name);