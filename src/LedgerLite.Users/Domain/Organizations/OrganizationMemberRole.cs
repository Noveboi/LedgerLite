using Ardalis.SmartEnum;

namespace LedgerLite.Users.Domain.Organizations;

public sealed class OrganizationMemberRole(string name, int value) : SmartEnum<OrganizationMemberRole>(name, value)
{
    public static readonly OrganizationMemberRole Owner = new(nameof(Owner), 1);
    public static readonly OrganizationMemberRole Admin = new(nameof(Admin), 2);
    public static readonly OrganizationMemberRole Member = new(nameof(Member), 3);
}