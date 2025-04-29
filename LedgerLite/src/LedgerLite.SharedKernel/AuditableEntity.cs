namespace LedgerLite.SharedKernel;

public class AuditableEntity : Entity, IAuditable
{
    public DateTime CreatedAtUtc { get; set; } = default;
    public DateTime ModifiedAtUtc { get; set; } = default;
}