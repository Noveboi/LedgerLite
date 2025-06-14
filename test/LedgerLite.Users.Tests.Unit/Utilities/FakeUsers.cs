﻿using Bogus;
using LedgerLite.Users.Domain;

namespace LedgerLite.Users.Tests.Unit.Utilities;

public sealed class FakeUserBuilder
{
    internal Guid? OrganizationMemberId { get; private set; }

    public FakeUserBuilder WithOrganizationMemberId(Guid? id = null)
    {
        OrganizationMemberId = id ?? Guid.NewGuid();
        return this;
    }
}

public static class FakeUsers
{
    private static Faker<User> Faker(FakeUserBuilder options)
    {
        return new Faker<User>()
            .RuleFor(x => x.UserName, f => f.Internet.UserName())
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.OrganizationMemberId, value: options.OrganizationMemberId);
    }

    public static User Get(Action<FakeUserBuilder>? configure = null)
    {
        var options = new FakeUserBuilder();
        configure?.Invoke(obj: options);

        return Faker(options: options).Generate();
    }
}