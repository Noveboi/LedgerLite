﻿using Ardalis.Result;
using LedgerLite.SharedKernel.UseCases;
using LedgerLite.Users.Contracts;
using LedgerLite.Users.Contracts.Models;

namespace LedgerLite.Accounting.Core.Application.UseCases;

internal sealed class GetOrganizationFromUserUseCase(
    IUserRequests userRequests, 
    IOrganizationRequests organizationRequests) 
    : IApplicationUseCase<Guid, OrganizationDto>
{
    public async Task<Result<OrganizationDto>> HandleAsync(Guid request, CancellationToken token)
    {
        var userResult = await userRequests.GetUserByIdAsync(request, token);
        if (!userResult.IsSuccess)
        {
            return userResult.Map();
        }

        var user = userResult.Value;
        if (user.Organization?.Id is not { } organizationId)
        {
            return Result.Invalid(GetOrganizationFromUserUseCaseErrors.UserNotInOrganization(user));
        }

        var organizationResult = await organizationRequests.GetOrganizationByIdAsync(organizationId, token);
        if (!organizationResult.IsSuccess)
        {
            return organizationResult.Map();
        }

        return organizationResult.Value;
    }
}

internal static class GetOrganizationFromUserUseCaseErrors
{
    private const string UseCaseIdentifier = "GetOrganizationFromUser";

    public static ValidationError UserNotInOrganization(UserDto user) =>
        new(identifier: UseCaseIdentifier,
            errorMessage: $"User {user.Username} is not in organization.",
            errorCode: "ACCUSR-USER_NOT_IN_ORG",
            severity: ValidationSeverity.Error);
}