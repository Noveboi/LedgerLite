using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.SharedKernel.Identity;
using LedgerLite.Users.Application.Roles;
using LedgerLite.Users.Application.Users;
using LedgerLite.Users.Contracts;
using LedgerLite.Users.Contracts.Models;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Infrastructure;
using LedgerLite.Users.Integrations.Conversions;

namespace LedgerLite.Users.Endpoints.Organizations;

internal sealed record CreateOrganizationRequestDto(
    [property: FromClaim(claimType: LedgerClaims.UserId)]
    Guid UserId,
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
        var createResult = await HandleUseCaseAsync(req: req, ct: ct);
        if (!createResult.IsSuccess)
        {
            await SendResultAsync(result: createResult.ToMinimalApiResult());
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
        if (await unitOfWork.OrganizationRepository.NameExistsAsync(name: req.Name, token: ct))
            return Result.Conflict($"Organization with name '{req.Name}' already exists.");

        return await userService.GetByIdAsync(id: req.UserId, token: ct)
            .BindAsync(bindFunc: user => user.OrganizationMemberId is not null
                ? Result.Invalid(validationError: OrganizationErrors.CannotBeInTwoOrganizations(user: user))
                : Result.Success(value: user))
            .BindAsync(bindFunc: async user => await roles.GetByNameAsync(name: CommonRoles.Owner, ct: ct)
                .MapAsync(func: role => new { User = user, Role = role }))
            .BindAsync(bindFunc: state => Organization.Create(
                    creator: state.User,
                    creatorRole: state.Role,
                    name: req.Name)
                .Map(func: org => new { state.User, state.Role, Organization = org }))
            .BindAsync(bindFunc: state =>
            {
                unitOfWork.OrganizationRepository.Add(organization: state.Organization);
                return Result.Success(value: state);
            })
            .BindAsync(bindFunc: state => unitOfWork.SaveChangesAsync(token: ct)
                .MapAsync(func: () => state.Organization));
    }
}