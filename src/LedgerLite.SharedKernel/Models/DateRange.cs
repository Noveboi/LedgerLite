namespace LedgerLite.SharedKernel.Models;

public readonly record struct DateRange(DateOnly Start, DateOnly End)
{
    /// <summary>
    ///     Determines if the two date ranges overlap.
    /// </summary>
    /// <param name="other">The other date range to check for overlap.</param>
    /// <returns>The overlap range, or null if they do not overlap.</returns>
    public DateRange? GetOverlapWith(DateRange other)
    {
        // Ensure Start is before or equal to End for both ranges
        if (Start > End || other.Start > other.End)
            throw new InvalidOperationException("Start date must be less than or equal to End date in both ranges.");

        // Calculate the latest start and earliest end
        var overlapStart = Start > other.Start ? Start : other.Start;
        var overlapEnd = End < other.End ? End : other.End;

        // If the ranges overlap, the start must be less than or equal to the end
        if (overlapStart <= overlapEnd) return new DateRange(overlapStart, overlapEnd);

        return null;
    }
}