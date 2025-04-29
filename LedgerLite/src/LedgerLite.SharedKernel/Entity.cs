namespace LedgerLite.SharedKernel;

public abstract class Entity : IAuditable
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

    public DateTime CreatedAtUtc { get; set; } = default;
    public DateTime ModifiedAtUtc { get; set; } = default;
}