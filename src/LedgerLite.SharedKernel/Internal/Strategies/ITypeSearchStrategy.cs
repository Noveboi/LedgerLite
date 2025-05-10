namespace LedgerLite.SharedKernel.Internal.Strategies;

internal interface ITypeSearchStrategy
{
    IEnumerable<Type> Filter(Type[] types);
    IEnumerable<Type> GetInterfaces(Type type);
}