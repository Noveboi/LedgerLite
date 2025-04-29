namespace LedgerLite.SharedKernel;

public abstract class Entity
{
    protected Entity() { }
    protected Entity(Guid? id)
    {
        Id = (id == Guid.Empty ? GenerateId() : id) ?? GenerateId();
    }

    public Guid Id { get; private init; }

    public override bool Equals(object? obj)
    {
        if (obj is Entity entity)
            return Id == entity.Id;

        return false;
    }

    public override int GetHashCode() => Id.GetHashCode();
    private static Guid GenerateId() => Guid.CreateVersion7();
}