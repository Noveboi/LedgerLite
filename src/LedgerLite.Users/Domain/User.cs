﻿using LedgerLite.SharedKernel.Domain;
using LedgerLite.Users.Domain.Organizations;
using Microsoft.AspNetCore.Identity;

namespace LedgerLite.Users.Domain;

public sealed class User : IdentityUser<Guid>, IAuditable
{
    [ProtectedPersonalData] public string? FirstName { get; set; }

    [ProtectedPersonalData] public string? LastName { get; set; }

    public Guid? OrganizationMemberId { get; private set; }
    public OrganizationMember? OrganizationMember { get; private set; }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime ModifiedAtUtc { get; set; }

    public void RemoveMemberInformation()
    {
        OrganizationMemberId = null;
        OrganizationMember = null;
    }

    public bool IsUsingEmailAsUsername()
    {
        return Email == UserName;
    }
}