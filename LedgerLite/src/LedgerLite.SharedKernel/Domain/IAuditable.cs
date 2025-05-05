namespace LedgerLite.SharedKernel.Domain;

public interface IAuditable
{
    DateTime CreatedAtUtc { get; internal set; }
    DateTime ModifiedAtUtc { get; internal set; }
}