using Ardalis.Result;
using LedgerLite.Tests.Shared;
using LedgerLite.Users.Application.Roles;
using LedgerLite.Users.Application.Users;
using LedgerLite.Users.Contracts;
using LedgerLite.Users.Domain;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Endpoints.Organizations;
using LedgerLite.Users.Infrastructure;
using LedgerLite.Users.Infrastructure.Repositories;
using LedgerLite.Users.Tests.Unit.Utilities;

namespace LedgerLite.Users.Tests.Unit.Application;

public class CreateOrganizationUseCaseTests
{
    private static readonly Role Role = new(name: CommonRoles.Owner);
    private readonly IOrganizationRepository _organizationRepository = Substitute.For<IOrganizationRepository>();
    private readonly IRoleService _roleService = Substitute.For<IRoleService>();
    private readonly CreateOrganizationEndpoint _sut;
    private readonly IUsersUnitOfWork _unitOfWork = Substitute.For<IUsersUnitOfWork>();
    private readonly IUserService _userService = Substitute.For<IUserService>();

    public CreateOrganizationUseCaseTests()
    {
        _roleService.GetByNameAsync(name: CommonRoles.Owner, Arg.Any<CancellationToken>())
            .Returns(returnThis: Role);

        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Result.Success());
        _unitOfWork.OrganizationRepository.Returns(returnThis: _organizationRepository);
        _sut = new CreateOrganizationEndpoint(unitOfWork: _unitOfWork, roles: _roleService, userService: _userService);
    }

    [Fact]
    public async Task Conflict_WhenNameAlreadyExists()
    {
        _organizationRepository.NameExistsAsync(name: "Oh no!", Arg.Any<CancellationToken>())
            .Returns(returnThis: true);
        var request = new CreateOrganizationRequestDto(Guid.NewGuid(), Name: "Oh no!");

        var result = await _sut.HandleUseCaseAsync(req: request, ct: CancellationToken.None);

        result.Status.ShouldBe(expected: ResultStatus.Conflict);
        result.Errors.ShouldHaveSingleItem().ShouldMatch(regexPattern: "Oh no!");
    }

    [Fact]
    public async Task AddToRepository_WhenCreationSuccessful()
    {
        var user = ConfigureUser();
        var request = GetRequest(user: user, name: "Ok!");
        var result = await _sut.HandleUseCaseAsync(req: request, ct: CancellationToken.None);

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        _organizationRepository.Received(requiredNumberOfCalls: 1)
            .Add(Arg.Is<Organization>(o => o.Name == request.Name));
    }

    [Fact]
    public async Task SaveChanges_WhenCreationSuccessful()
    {
        var user = ConfigureUser();
        var request = GetRequest(user: user, name: "Ok!");

        var result = await _sut.HandleUseCaseAsync(req: request, ct: CancellationToken.None);

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        await _unitOfWork.Received(requiredNumberOfCalls: 1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Invalid_WhenUserAlreadyInOrganization()
    {
        var user = ConfigureUser(FakeUsers.Get(x => x.WithOrganizationMemberId()));
        var request = GetRequest(user: user, name: "No no!");

        var result = await _sut.HandleUseCaseAsync(req: request, ct: CancellationToken.None);

        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ShouldHaveError(OrganizationErrors.CannotBeInTwoOrganizations(user: user));
        ;
    }

    private static CreateOrganizationRequestDto GetRequest(User user, string name)
    {
        return new CreateOrganizationRequestDto(UserId: user.Id, Name: name);
    }

    private User ConfigureUser(User? user = null)
    {
        user ??= FakeUsers.Get();
        _userService
            .GetByIdAsync(id: user.Id, Arg.Any<CancellationToken>())
            .Returns(returnThis: user);

        return user;
    }
}