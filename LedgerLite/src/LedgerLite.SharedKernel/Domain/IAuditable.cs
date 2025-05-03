namespace LedgerLite.SharedKernel.Domain;

public interface IAuditable
{
    DateTime CreatedAtUtc { get; }
    DateTime ModifiedAtUtc { get; }
}