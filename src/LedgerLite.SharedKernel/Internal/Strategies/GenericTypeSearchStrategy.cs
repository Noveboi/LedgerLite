namespace LedgerLite.SharedKernel.Internal.Strategies;

internal sealed class GenericTypeSearchStrategy(Type searchTarget) : ITypeSearchStrategy
{
    public IEnumerable<Type> Filter(Type[] types)
    {
        return types
            .Where(t => t is { IsAbstract: false, IsClass: true } && t
                .GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == searchTarget)
            );
    }

    public IEnumerable<Type> GetInterfaces(Type type)
    {
        return type.GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == searchTarget);
    }
}