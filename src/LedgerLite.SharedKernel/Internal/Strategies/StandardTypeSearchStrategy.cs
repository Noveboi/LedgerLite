namespace LedgerLite.SharedKernel.Internal.Strategies;

internal sealed class StandardTypeSearchStrategy(Type searchTarget) : ITypeSearchStrategy
{
    public IEnumerable<Type> Filter(Type[] types)
    {
        return types.Where(t => t is { IsAbstract: false, IsClass: true } &&
                                searchTarget.IsAssignableFrom(t));
    }

    public IEnumerable<Type> GetInterfaces(Type type)
    {
        return type.GetInterfaces();
    }
}