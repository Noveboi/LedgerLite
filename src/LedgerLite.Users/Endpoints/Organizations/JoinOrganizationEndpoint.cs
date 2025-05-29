using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.SharedKernel.Domain.Errors;
using LedgerLite.SharedKernel.Identity;
using LedgerLite.Users.Application.Users;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Infrastructure;
using LedgerLite.Users.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LedgerLite.Users.Endpoints.Organizations;

internal sealed record JoinOrganizationRequestDto(
    [property: FromClaim(LedgerClaims.UserId)] Guid UserId,
    [property: FromRoute] Guid OrganizationId);

internal sealed class JoinOrganizationEndpoint(
    IUserService userService,
    IUserUnitOfWork unitOfWork) : Endpoint<JoinOrganizationRequestDto>
{
    private readonly IOrganizationRepository _organizationRepository = unitOfWork.OrganizationRepository;
    
    public override void Configure()
    {
        Put("/{organizationId:guid}/join");
        Description(x => x.Accepts<JoinOrganizationRequestDto>());
        Group<OrganizationEndpointGroup>();
    }

    public override async Task HandleAsync(JoinOrganizationRequestDto req, CancellationToken ct)
    {
        var result = await HandleUseCaseAsync(req, ct);
        if (!result.IsSuccess)
        {
            await SendResultAsync(result.ToMinimalApiResult());
            return;
        }

        await SendOkAsync(ct);
    }

    private async Task<Result<OrganizationMember>> HandleUseCaseAsync(
        JoinOrganizationRequestDto request,
        CancellationToken token) =>
        await userService.GetByIdAsync(request.UserId, token)
            .BindAsync(async user => await _organizationRepository.GetByIdAsync(request.OrganizationId, token) is { } org
                ? Result.Success(new { User = user, Organization = org })
                : Result.NotFound(CommonErrors.NotFound<Organization>(request.OrganizationId)))
            .BindAsync(state => OrganizationMember.Create(state.User, OrganizationMemberRole.Member, state.Organization.Id)
                .Map(member => new { state.User, state.Organization, Member = member }))
            .BindAsync(state => state.Organization.AddMember(state.Member)
                .Map(() => state.Member))
            .BindAsync(member => unitOfWork.SaveChangesAsync(token)
                .MapAsync(() => member));
}