using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.SharedKernel.Domain.Errors;
using LedgerLite.SharedKernel.Identity;
using LedgerLite.Users.Application.Roles;
using LedgerLite.Users.Application.Users;
using LedgerLite.Users.Contracts;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Infrastructure;
using LedgerLite.Users.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LedgerLite.Users.Endpoints.Organizations;

internal sealed record JoinOrganizationRequestDto(
    [property: FromClaim(claimType: LedgerClaims.UserId)]
    Guid UserId,
    [property: FromRoute] Guid OrganizationId);

internal sealed class JoinOrganizationEndpoint(
    IUserService userService,
    IUsersUnitOfWork unitOfWork,
    IRoleService roles) : Endpoint<JoinOrganizationRequestDto>
{
    private readonly IOrganizationRepository _organizations = unitOfWork.OrganizationRepository;

    public override void Configure()
    {
        Put("/{organizationId:guid}/join");
        Description(x => x.Accepts<JoinOrganizationRequestDto>());
        Group<OrganizationEndpointGroup>();
    }

    public override async Task HandleAsync(JoinOrganizationRequestDto req, CancellationToken ct)
    {
        var result = await HandleUseCaseAsync(request: req, token: ct);
        if (!result.IsSuccess)
        {
            await SendResultAsync(result.ToMinimalApiResult());
            return;
        }

        await SendOkAsync(cancellation: ct);
    }

    public async Task<Result<OrganizationMember>> HandleUseCaseAsync(
        JoinOrganizationRequestDto request,
        CancellationToken token)
    {
        return await userService.GetByIdAsync(id: request.UserId, token: token)
            .BindAsync(async user =>
                await _organizations.GetByIdAsync(id: request.OrganizationId, token: token) is { } org
                    ? Result.Success(new { User = user, Organization = org })
                    : Result.NotFound(CommonErrors.NotFound<Organization>(id: request.OrganizationId)))
            .BindAsync(state => roles.GetByNameAsync(name: CommonRoles.Viewer, ct: token)
                .MapAsync(role => new { state.User, state.Organization, Role = role }))
            .BindAsync(state => OrganizationMember
                .Create(user: state.User, organization: state.Organization, role: state.Role)
                .Map(member => new { state.User, state.Organization, Member = member }))
            .BindAsync(state => state.Organization.AddMember(member: state.Member)
                .Map(() => state.Member))
            .BindAsync(member => unitOfWork.SaveChangesAsync(token: token)
                .MapAsync(() => member));
    }
}