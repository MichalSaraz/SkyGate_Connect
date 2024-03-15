using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Extensions;

public static class PropertyBuilderExtensions
{
    public static void SetValueComparerForDictionary<T>(this PropertyBuilder<Dictionary<string, T>> propertyBuilder)
    {
        propertyBuilder.Metadata.SetValueComparer(
            new ValueComparer<Dictionary<string, T>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.Key.GetHashCode(), v.Value.GetHashCode())),
                c => c.ToDictionary(x => x.Key, x => x.Value)
            )
        );
    }
    
    public static void SetValueComparerForList<T>(this PropertyBuilder<List<T>> propertyBuilder)
    {
        propertyBuilder.Metadata.SetValueComparer(
            new ValueComparer<List<T>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()
            )
        );
    }
}