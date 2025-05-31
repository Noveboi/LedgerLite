using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.SharedKernel.Identity;
using LedgerLite.Users.Application.Roles;
using LedgerLite.Users.Application.Users;
using LedgerLite.Users.Contracts.Models;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Infrastructure;
using LedgerLite.Users.Integrations.Conversions;

namespace LedgerLite.Users.Endpoints.Organizations;

internal sealed record CreateOrganizationRequestDto(
    [property: FromClaim(LedgerClaims.UserId)] Guid UserId,
    string Name);

internal sealed class CreateOrganizationEndpoint(
    IUsersUnitOfWork unitOfWork,
    IRoleService roles,
    IUserService userService) 
    : Endpoint<CreateOrganizationRequestDto, OrganizationDto>
{
    public override void Configure()
    {
        Post("");
        Group<OrganizationEndpointGroup>();
    }

    public override async Task HandleAsync(CreateOrganizationRequestDto req, CancellationToken ct)
    {
        var createResult = await HandleUseCaseAsync(req, ct);
        if (!createResult.IsSuccess)
        {
            await SendResultAsync(createResult.ToMinimalApiResult());
            return;
        }

        var organization = createResult.Value;

        await SendCreatedAtAsync<GetOrganizationEndpoint>(
            routeValues: new { organization.Id },
            responseBody: organization.ToDto(),
            cancellation: ct);
    }

    public async Task<Result<Organization>> HandleUseCaseAsync(CreateOrganizationRequestDto req, CancellationToken ct)
    {
        if (await unitOfWork.OrganizationRepository.NameExistsAsync(req.Name, ct))
            return Result.Conflict($"Organization with name '{req.Name}' already exists.");
        
        return await userService.GetByIdAsync(req.UserId, ct)
            .BindAsync(user => user.OrganizationMemberId is not null 
                ? Result.Invalid(OrganizationErrors.CannotBeInTwoOrganizations(user))
                : Result.Success(user))
            .BindAsync(async user => await roles.GetByNameAsync(CommonRoles.Owner, ct)
                .MapAsync(role => new { User = user, Role = role }))
            .BindAsync(state => Organization.Create(
                creator: state.User,
                creatorRole: state.Role,
                name: req.Name)
                .Map(org => new { state.User, state.Role, Organization = org }))
            .BindAsync(state =>
            {
                unitOfWork.OrganizationRepository.Add(state.Organization);
                return Result.Success(state);
            })
            .BindAsync(state => unitOfWork.SaveChangesAsync(ct)
                .MapAsync(() => state.Organization));
    }
}