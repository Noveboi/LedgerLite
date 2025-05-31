using Bogus;

namespace LedgerLite.Tests.Shared;

public sealed class PrivateFaker<T> : Faker<T> where T : class
{
    public PrivateFaker()
    {
    }

    public PrivateFaker(Binder binder) : base(locale: "en", binder: binder)
    {
    }

    public PrivateFaker<T> UsePrivateConstructor()
    {
        return (CustomInstantiator(factoryMethod: _ =>
                (Activator.CreateInstance(type: typeof(T), nonPublic: true) as T)!)
            as PrivateFaker<T>)!;
    }
}