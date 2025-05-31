namespace LedgerLite.SharedKernel.Internal.Strategies;

internal sealed class GenericTypeSearchStrategy(Type searchTarget) : ITypeSearchStrategy
{
    public IEnumerable<Type> Filter(Type[] types)
    {
        return types
            .Where(predicate: t => t is { IsAbstract: false, IsClass: true } && t
                .GetInterfaces()
                .Any(predicate: i => i.IsGenericType && i.GetGenericTypeDefinition() == searchTarget)
            );
    }

    public IEnumerable<Type> GetInterfaces(Type type)
    {
        return type.GetInterfaces()
            .Where(predicate: i => i.IsGenericType && i.GetGenericTypeDefinition() == searchTarget);
    }
}