﻿using LedgerLite.Users.Contracts.Models;
using LedgerLite.Users.Domain.Organizations;

namespace LedgerLite.Users.Integrations.Conversions;

internal static class OrganizationsToDtoConversions
{
    public static OrganizationDto ToDto(this Organization org) => new(
        Id: org.Id,
        Name: org.Name);
}