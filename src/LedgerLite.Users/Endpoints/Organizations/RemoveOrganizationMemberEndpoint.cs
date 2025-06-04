using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.SharedKernel.Domain.Errors;
using LedgerLite.SharedKernel.Identity;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Users.Endpoints.Organizations;

internal sealed record RemoveOrganizationMemberRequest(
    [property: FromClaim(LedgerClaims.UserId)] Guid UserId,
    [property: RouteParam] Guid OrganizationId,
    [property: RouteParam] Guid MemberId);

internal sealed class RemoveOrganizationMemberEndpoint(
    IUsersUnitOfWork unitOfWork,
    UsersDbContext context) 
    : Endpoint<RemoveOrganizationMemberRequest>
{
    public override void Configure()
    {
        Delete("{memberId:guid}");
        Group<OrganizationMemberEndpointGroup>();
    }

    public override async Task HandleAsync(RemoveOrganizationMemberRequest req, CancellationToken ct)
    {
        var result = await HandleUseCaseAsync(req, ct);
        if (!result.IsSuccess)
        {
            await SendResultAsync(result.ToMinimalApiResult());
            return;
        }

        await SendOkAsync(ct);
    }

    public async Task<Result> HandleUseCaseAsync(RemoveOrganizationMemberRequest req, CancellationToken ct) =>
        await GetOrganizationAsync(req.OrganizationId, ct)
            .BindAsync(async org => await context.OrganizationMembers
                .FirstOrDefaultAsync(x => x.Id == req.MemberId, ct) is { } member
                ? Result.Success(new { Org = org, Member = member })
                : Result.NotFound(CommonErrors.NotFound<OrganizationMember>(req.MemberId)))
            .BindAsync(state => state.Org.RemoveMember(state.Member))
            .BindAsync(_ => unitOfWork.SaveChangesAsync(ct));

    private async Task<Result<Organization>> GetOrganizationAsync(Guid orgId, CancellationToken ct) =>
        await context.Organizations.FirstOrDefaultAsync(x => x.Id == orgId, ct) is { } org
            ? Result.Success(org)
            : Result.NotFound(CommonErrors.NotFound<Organization>(orgId));
}