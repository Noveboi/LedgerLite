using Ardalis.Result;

namespace LedgerLite.Users.Domain;

internal static class OrganizationErrors
{
    private const string OrganizationIdentifier = "Organization";

    public static ValidationError CannotTransferUserToSameOrganization(User user) =>
        new(identifier: OrganizationIdentifier,
            errorMessage: $"User '{user.UserName}' is already in this organization.",
            errorCode: "ORG-ALREADY_IN",
            severity: ValidationSeverity.Error);
}