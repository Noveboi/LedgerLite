using Bogus;
using LedgerLite.Users.Domain;

namespace LedgerLite.Users.Tests.Unit.Utilities;

public static class FakeUsers
{
    private static Faker<User> Faker() => new Faker<User>()
        .RuleFor(x => x.UserName, f => f.Internet.UserName())
        .RuleFor(x => x.Email, f => f.Internet.Email());

    public static User Get() => Faker().Generate();
}