using Ardalis.Result;

namespace LedgerLite.Users.Domain.Organizations;

internal static class OrganizationErrors
{
    private const string OrganizationIdentifier = "Organization";

    public static ValidationError MemberAlreadyInOrganization(OrganizationMember member) =>
        new(identifier: OrganizationIdentifier,
            errorMessage: $"User '{member.User.UserName}' is already in this organization.",
            errorCode: "ORG-ALREADY_IN",
            severity: ValidationSeverity.Error);

    public static ValidationError MemberNotInOrganization(OrganizationMember member) =>
        new(identifier: OrganizationIdentifier,
            errorMessage: $"User '{member.User.UserName}' is not in organization.",
            errorCode: "ORG-MEMBER_NOT_FOUND",
            severity: ValidationSeverity.Error);

    public static ValidationError NameIsTheSame() =>
        new(identifier: OrganizationIdentifier,
            errorMessage: "Cannot rename organization using the same name.",
            errorCode: "ORG-SAME_NAME",
            severity: ValidationSeverity.Error);

    public static ValidationError CannotBeInTwoOrganizations(User user) =>
        new(identifier: OrganizationIdentifier,
            errorMessage: $"{user.UserName} is already in an organization.",
            errorCode: "ORG-ALREADY_IN_ONE",
            severity: ValidationSeverity.Error);
}