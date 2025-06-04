using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.SharedKernel.Domain.Errors;
using LedgerLite.SharedKernel.Identity;
using LedgerLite.Users.Authorization;
using LedgerLite.Users.Contracts.Models;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Infrastructure;
using LedgerLite.Users.Integrations.Conversions;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Users.Endpoints.Organizations;

internal sealed record GetOrganizationRequest(
    [property: FromClaim(LedgerClaims.UserId)] Guid UserId,
    [property: RouteParam] Guid OrganizationId);

internal sealed class GetOrganizationMembersEndpoint(UsersDbContext context) 
    : Endpoint<GetOrganizationRequest, IEnumerable<UserDto>>
{
    public override void Configure()
    {
        Get("");
        Group<OrganizationMemberEndpointGroup>();
    }

    public override async Task HandleAsync(GetOrganizationRequest req, CancellationToken ct)
    {
        var result = await HandleUseCaseAsync(req, ct);
        if (!result.IsSuccess)
        {
            await SendResultAsync(result.ToMinimalApiResult());
            return;
        }

        await SendAsync(result.Value.Select(x => x.ToUserDto()), cancellation: ct);
    }

    public async Task<Result<IReadOnlyCollection<OrganizationMember>>> HandleUseCaseAsync(
        GetOrganizationRequest req,
        CancellationToken ct)
    {
        return (await context.Organizations
                .AsSplitQuery()
                .Include(x => x.Members.Where(m => m.User.Id != req.UserId))
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == req.OrganizationId, ct) is { } organization
                ? Result.Success(organization)
                : Result.NotFound(CommonErrors.NotFound<Organization>(req.OrganizationId)))
            .Map(org => org.Members);
    }
    
}