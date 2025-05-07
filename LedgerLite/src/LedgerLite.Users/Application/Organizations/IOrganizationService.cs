using Ardalis.Result;
using LedgerLite.Users.Application.Organizations.Requests;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Infrastructure;

namespace LedgerLite.Users.Application.Organizations;

public interface IOrganizationService
{
    Task<Result<Organization>> CreateAsync(CreateOrganizationRequest request, CancellationToken token);
    Task<Result<Organization>> RemoveAsync(RemoveOrganizationRequest request, CancellationToken token);
}

public sealed class OrganizationService(IUserUnitOfWork unitOfWork) : IOrganizationService
{
    public Task<Result<Organization>> CreateAsync(CreateOrganizationRequest req, CancellationToken token) =>
        Organization.Create(req.Name)
            .Bind(org => unitOfWork.OrganizationRepository.Add(org))
            .BindAsync(org => unitOfWork.SaveChangesAsync(token).MapAsync(() => org));

    public Task<Result<Organization>> RemoveAsync(RemoveOrganizationRequest request, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}