﻿using System.Security.Claims;

namespace LedgerLite.SharedKernel.Identity;

public static class LedgerClaims
{
    public const string UserId = ClaimTypes.NameIdentifier;
    public const string Role = ClaimTypes.Role;
}