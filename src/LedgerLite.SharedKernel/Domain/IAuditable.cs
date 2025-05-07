namespace LedgerLite.SharedKernel.Domain;

public interface IAuditable
{
    DateTime CreatedAtUtc { get; set; }
    DateTime ModifiedAtUtc { get; set; }
}