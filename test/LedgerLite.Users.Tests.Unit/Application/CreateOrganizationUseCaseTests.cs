using Ardalis.Result;
using LedgerLite.Users.Application.Organizations;
using LedgerLite.Users.Application.Organizations.Requests;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Infrastructure;
using LedgerLite.Users.Infrastructure.Repositories;

namespace LedgerLite.Users.Tests.Unit.Application;

public class CreateOrganizationUseCaseTests
{
    private readonly IUserUnitOfWork _unitOfWork = Substitute.For<IUserUnitOfWork>();
    private readonly IOrganizationRepository _organizationRepository = Substitute.For<IOrganizationRepository>();
    private readonly OrganizationService _sut;

    private static readonly CreateOrganizationRequest _request = new("Ok!");

    public CreateOrganizationUseCaseTests()
    {
        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Result.Success());
        _unitOfWork.OrganizationRepository.Returns(_organizationRepository);
        _sut = new OrganizationService(_unitOfWork);
    }

    [Fact]
    public async Task Conflict_WhenNameAlreadyExists()
    {
        _organizationRepository.NameExistsAsync("Oh no!", Arg.Any<CancellationToken>()).Returns(true);
        var request = new CreateOrganizationRequest("Oh no!");

        var result = await _sut.CreateAsync(request, CancellationToken.None);
        
        result.Status.ShouldBe(ResultStatus.Conflict);
        result.Errors.ShouldHaveSingleItem().ShouldMatch("Oh no!");
    }

    [Fact]
    public async Task AddToRepository_WhenCreationSuccessful()
    {
        var result = await _sut.CreateAsync(_request, CancellationToken.None);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        _organizationRepository.Received(1).Add(Arg.Is<Organization>(o => o.Name == _request.Name));
    }

    [Fact]
    public async Task SaveChanges_WhenCreationSuccessful()
    {
        var result = await _sut.CreateAsync(_request, CancellationToken.None);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}