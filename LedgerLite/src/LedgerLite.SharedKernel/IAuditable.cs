namespace LedgerLite.SharedKernel;

public interface IAuditable
{
    DateTime CreatedAtUtc { get; }
    DateTime ModifiedAtUtc { get; }
}