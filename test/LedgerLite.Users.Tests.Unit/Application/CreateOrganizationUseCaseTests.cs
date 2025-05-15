using Ardalis.Result;
using LedgerLite.Tests.Shared;
using LedgerLite.Users.Application.Organizations;
using LedgerLite.Users.Application.Organizations.Requests;
using LedgerLite.Users.Application.Users;
using LedgerLite.Users.Domain;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Infrastructure;
using LedgerLite.Users.Infrastructure.Repositories;
using LedgerLite.Users.Tests.Unit.Utilities;

namespace LedgerLite.Users.Tests.Unit.Application;

public class CreateOrganizationUseCaseTests
{
    private readonly IUserService _userService = Substitute.For<IUserService>();
    private readonly IUserUnitOfWork _unitOfWork = Substitute.For<IUserUnitOfWork>();
    private readonly IOrganizationRepository _organizationRepository = Substitute.For<IOrganizationRepository>();
    private readonly OrganizationService _sut;

    public CreateOrganizationUseCaseTests()
    {
        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Result.Success());
        _unitOfWork.OrganizationRepository.Returns(_organizationRepository);
        _sut = new OrganizationService(_unitOfWork, _userService);
    }

    [Fact]
    public async Task Conflict_WhenNameAlreadyExists()
    {
        _organizationRepository.NameExistsAsync("Oh no!", Arg.Any<CancellationToken>()).Returns(true);
        var request = new CreateOrganizationRequest(Guid.NewGuid(), "Oh no!");

        var result = await _sut.CreateAsync(request, CancellationToken.None);
        
        result.Status.ShouldBe(ResultStatus.Conflict);
        result.Errors.ShouldHaveSingleItem().ShouldMatch("Oh no!");
    }

    [Fact]
    public async Task AddToRepository_WhenCreationSuccessful()
    {
        var user = ConfigureUser();
        var request = GetRequest(user, "Ok!");
        var result = await _sut.CreateAsync(request, CancellationToken.None);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        _organizationRepository.Received(1).Add(Arg.Is<Organization>(o => o.Name == request.Name));
    }

    [Fact]
    public async Task SaveChanges_WhenCreationSuccessful()
    {
        var user = ConfigureUser();
        var request = GetRequest(user, "Ok!");
        
        var result = await _sut.CreateAsync(request, CancellationToken.None);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Invalid_WhenUserAlreadyInOrganization()
    {
        var user = ConfigureUser(FakeUsers.Get(x => x.WithOrganizationMemberId()));
        var request = GetRequest(user, "No no!");

        var result = await _sut.CreateAsync(request, CancellationToken.None);

        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ShouldHaveError(OrganizationErrors.CannotBeInTwoOrganizations(user));
;    }

    private static CreateOrganizationRequest GetRequest(User user, string name) => 
        new(user.Id, name);

    private User ConfigureUser(User? user = null)
    {
        user ??= FakeUsers.Get();
        _userService
            .GetByIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        return user;
    }
}