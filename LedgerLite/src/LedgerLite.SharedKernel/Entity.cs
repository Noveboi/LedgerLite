namespace LedgerLite.SharedKernel;

public abstract class Entity
{
    private Entity() { }
    protected Entity(Guid? id)
    {
    }

    public Guid Id { get; private init; }
}