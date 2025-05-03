using KellermanSoftware.CompareNetObjects;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.Tests.Shared;

public static class AssertionEquivalenceExtensions
{
    public static void EquivalentTo<T>(this T subject, T other, Action<ComparisonConfig>? configure = null)
    {
        var config = new ComparisonConfig
        {
            MaxDifferences = 50,
            IgnoreCollectionOrder = true,
        };
        
        configure?.Invoke(config);
        subject.ShouldCompare(other, compareConfig: config);
    }
    
    public static void EquivalentToEntity<T>(this T subject, T other, Action<ComparisonConfig>? configure = null)
        where T : Entity
    {
        var config = new ComparisonConfig
        {
            MaxDifferences = 50,
            IgnoreCollectionOrder = true,
        };
        
        config.IgnoreProperty<T>(x => x.Id);
        
        configure?.Invoke(config);
        subject.ShouldCompare(other, compareConfig: config);
    }
}