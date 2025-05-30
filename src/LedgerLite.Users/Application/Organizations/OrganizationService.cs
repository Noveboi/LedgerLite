using Ardalis.Result;
using LedgerLite.Users.Application.Organizations.Requests;
using LedgerLite.Users.Application.Users;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Infrastructure;

namespace LedgerLite.Users.Application.Organizations;

internal sealed class OrganizationService(
    IUserUnitOfWork unitOfWork,
    IUserService userService) : IOrganizationService
{
    public async Task<Result<Organization>> CreateAsync(CreateOrganizationRequest req, CancellationToken token)
    {
        if (await unitOfWork.OrganizationRepository.NameExistsAsync(req.Name, token))
            return Result.Conflict($"Organization with name '{req.Name}' already exists.");
        
        return await Organization.Create(req.Name)
            .BindAsync(async org => await userService.GetByIdAsync(req.UserId, token)
                .MapAsync(user => new { Organization = org, User = user}))
            .BindAsync(state => state.User.OrganizationMemberId is not null 
                ? Result.Invalid(OrganizationErrors.CannotBeInTwoOrganizations(state.User))
                : Result.Success(state))
            .BindAsync(state => OrganizationMember.Create(
                user: state.User, 
                organizationId: state.Organization.Id).Map(member => new
                {
                    state.Organization,
                    Member = member
                }))
            .BindAsync(state => state.Organization.AddMember(state.Member).Map(() => state))
            .BindAsync(state =>
            {
                unitOfWork.OrganizationRepository.Add(state.Organization);
                return Result.Success(state);
            })
            .BindAsync(state => unitOfWork.SaveChangesAsync(token).MapAsync(() => state.Organization));
    }

    public Task<Result<Organization>> RemoveAsync(RemoveOrganizationRequest request, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}