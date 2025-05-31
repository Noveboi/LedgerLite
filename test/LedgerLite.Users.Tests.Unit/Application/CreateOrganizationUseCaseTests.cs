using Ardalis.Result;
using LedgerLite.Tests.Shared;
using LedgerLite.Users.Application.Roles;
using LedgerLite.Users.Application.Users;
using LedgerLite.Users.Domain;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Endpoints.Organizations;
using LedgerLite.Users.Infrastructure;
using LedgerLite.Users.Infrastructure.Repositories;
using LedgerLite.Users.Tests.Unit.Utilities;

namespace LedgerLite.Users.Tests.Unit.Application;

public class CreateOrganizationUseCaseTests
{
    private readonly IRoleService _roleService = Substitute.For<IRoleService>();
    private readonly IUserService _userService = Substitute.For<IUserService>();
    private readonly IUsersUnitOfWork _unitOfWork = Substitute.For<IUsersUnitOfWork>();
    private readonly IOrganizationRepository _organizationRepository = Substitute.For<IOrganizationRepository>();
    private readonly CreateOrganizationEndpoint _sut;

    private static readonly Role Role = new(CommonRoles.Owner);

    public CreateOrganizationUseCaseTests()
    {
        _roleService.GetByNameAsync(CommonRoles.Owner, Arg.Any<CancellationToken>()).Returns(Role);
        
        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Result.Success());
        _unitOfWork.OrganizationRepository.Returns(_organizationRepository);
        _sut = new CreateOrganizationEndpoint(_unitOfWork, _roleService, _userService);
    }

    [Fact]
    public async Task Conflict_WhenNameAlreadyExists()
    {
        _organizationRepository.NameExistsAsync("Oh no!", Arg.Any<CancellationToken>()).Returns(true);
        var request = new CreateOrganizationRequestDto(Guid.NewGuid(), "Oh no!");

        var result = await _sut.HandleUseCaseAsync(request, CancellationToken.None);
        
        result.Status.ShouldBe(ResultStatus.Conflict);
        result.Errors.ShouldHaveSingleItem().ShouldMatch("Oh no!");
    }

    [Fact]
    public async Task AddToRepository_WhenCreationSuccessful()
    {
        var user = ConfigureUser();
        var request = GetRequest(user, "Ok!");
        var result = await _sut.HandleUseCaseAsync(request, CancellationToken.None);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        _organizationRepository.Received(1).Add(Arg.Is<Organization>(o => o.Name == request.Name));
    }

    [Fact]
    public async Task SaveChanges_WhenCreationSuccessful()
    {
        var user = ConfigureUser();
        var request = GetRequest(user, "Ok!");
        
        var result = await _sut.HandleUseCaseAsync(request, CancellationToken.None);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Invalid_WhenUserAlreadyInOrganization()
    {
        var user = ConfigureUser(FakeUsers.Get(x => x.WithOrganizationMemberId()));
        var request = GetRequest(user, "No no!");

        var result = await _sut.HandleUseCaseAsync(request, CancellationToken.None);

        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ShouldHaveError(OrganizationErrors.CannotBeInTwoOrganizations(user));
;    }

    private static CreateOrganizationRequestDto GetRequest(User user, string name) => 
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