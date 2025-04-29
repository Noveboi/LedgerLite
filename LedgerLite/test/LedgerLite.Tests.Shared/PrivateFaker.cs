using Bogus;

namespace LedgerLite.Tests.Shared;

public sealed class PrivateFaker<T> : Faker<T> where T : class 
{
    public PrivateFaker<T> UsePrivateConstructor() =>
        (base.CustomInstantiator(_ => (Activator.CreateInstance(typeof(T), nonPublic: true) as T)! )
            as PrivateFaker<T>)!;
}