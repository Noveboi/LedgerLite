namespace LedgerLite.SharedKernel.Domain;

public abstract class AuditableEntity : Entity, IAuditable
{
    public DateTime CreatedAtUtc { get; set; } = default;
    public DateTime ModifiedAtUtc { get; set; } = default;
}