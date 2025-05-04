namespace LedgerLite.SharedKernel.Domain;

public abstract class Entity
{
    protected Entity()
    {
        Id = GenerateId();
    }
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

    public static bool operator ==(Entity? a, Entity? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity? a, Entity? b) => !(a == b);

    public override int GetHashCode() => Id.GetHashCode();
    private static Guid GenerateId() => Guid.CreateVersion7();
}