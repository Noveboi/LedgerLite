using LedgerLite.SharedKernel.Internal.Strategies;

namespace LedgerLite.SharedKernel.Internal;

public sealed class AssemblyScanStrategyBuilder
{
    internal Type BaseType { get; private set; } = null!;
    internal bool ShouldRegisterInterface { get; private set; } = true;
    internal bool ShouldRegisterImplementation { get; private set; } = false;

    internal ITypeSearchStrategy GetTypeSearchStrategy()
    {
        if (BaseType is null)
            throw new InvalidOperationException("Base type should not be null");

        if (BaseType.IsGenericType)
            return new GenericTypeSearchStrategy(BaseType);

        return new StandardTypeSearchStrategy(BaseType);
    }

    public AssemblyScanStrategyBuilder Implementing<TBase>()
    {
        BaseType = typeof(TBase);
        return this;
    }

    public AssemblyScanStrategyBuilder RegisterImplementationOnly()
    {
        ShouldRegisterImplementation = true;
        ShouldRegisterInterface = false;
        return this;
    }

    public AssemblyScanStrategyBuilder RegisterImplementationAsWell()
    {
        ShouldRegisterImplementation = true;
        return this;
    }
    
    public AssemblyScanStrategyBuilder Implementing(Type type)
    {
        BaseType = type;
        return this;
    }
}