using Ardalis.Result;
using LedgerLite.Users.Application.Organizations.Requests;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Infrastructure;

namespace LedgerLite.Users.Application.Organizations;

public sealed class OrganizationService(IUserUnitOfWork unitOfWork) : IOrganizationService
{
    public async Task<Result<Organization>> CreateAsync(CreateOrganizationRequest req, CancellationToken token)
    {
        if (await unitOfWork.OrganizationRepository.NameExistsAsync(req.Name, token))
        {
            return Result.Conflict($"Organization with name '{req.Name}' already exists.");
        }
        
        return await Organization.Create(req.Name)
            .Bind(org =>
            {
                unitOfWork.OrganizationRepository.Add(org);
                return Result.Success(org);
            })
            .BindAsync(org => unitOfWork.SaveChangesAsync(token).MapAsync(() => org));
    }

    public Task<Result<Organization>> RemoveAsync(RemoveOrganizationRequest request, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}