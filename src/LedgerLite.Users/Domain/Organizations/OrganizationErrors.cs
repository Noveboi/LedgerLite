using Ardalis.Result;

namespace LedgerLite.Users.Domain.Organizations;

internal static class OrganizationErrors
{
    private const string OrganizationIdentifier = "Organization";

    public static ValidationError MemberAlreadyInOrganization(OrganizationMember member)
    {
        return new ValidationError(OrganizationIdentifier,
            $"User '{member.User.UserName}' is already in this organization.",
            "ORG-ALREADY_IN",
            ValidationSeverity.Error);
    }

    public static ValidationError MemberNotInOrganization(OrganizationMember member)
    {
        return new ValidationError(OrganizationIdentifier,
            $"User '{member.User.UserName}' is not in organization.",
            "ORG-MEMBER_NOT_FOUND",
            ValidationSeverity.Error);
    }

    public static ValidationError NameIsTheSame()
    {
        return new ValidationError(OrganizationIdentifier,
            "Cannot rename organization using the same name.",
            "ORG-SAME_NAME",
            ValidationSeverity.Error);
    }

    public static ValidationError CannotBeInTwoOrganizations(User user)
    {
        return new ValidationError(OrganizationIdentifier,
            $"{user.UserName} is already in an organization.",
            "ORG-ALREADY_IN_ONE",
            ValidationSeverity.Error);
    }

    public static ValidationError MemberDoesNotHaveRole(OrganizationMember member)
    {
        return new ValidationError(OrganizationIdentifier,
            $"Cannot add member '{member.User.UserName}' without a role.",
            "ORG-NO_ROLE",
            ValidationSeverity.Error);
    }

    public static ValidationError AlreadyHasOwner()
    {
        return new ValidationError(OrganizationIdentifier,
            "Organization already has an owner.",
            "ORG-OWNER",
            ValidationSeverity.Error);
    }
}