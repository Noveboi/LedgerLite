using Ardalis.Result;
using LedgerLite.SharedKernel.Domain;
using Microsoft.AspNetCore.Identity;

namespace LedgerLite.Users.Domain;

internal sealed class User : IdentityUser<Guid>, IAuditable
{
    [ProtectedPersonalData]
    public string? FirstName { get; set; }
    
    [ProtectedPersonalData]
    public string? LastName { get; set; }
    
    public Guid? OrganizationId { get; private set; }


    public bool IsUsingEmailAsUsername() => Email == UserName;
    
    public Result RegisterToOrganization(Guid organizationId)
    {
        if (organizationId == OrganizationId)
            return Result.Invalid(OrganizationErrors.CannotTransferUserToSameOrganization(this));
        
        OrganizationId = organizationId;
        return Result.Success();
    }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime ModifiedAtUtc { get; set;}
}