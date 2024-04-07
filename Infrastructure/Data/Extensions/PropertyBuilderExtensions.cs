using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Extensions;

/// <summary>
/// Provides extension methods for the PropertyBuilder class.
/// </summary>
public static class PropertyBuilderExtensions
{
    /// <summary>
    /// Sets the value comparer for a dictionary property in a PropertyBuilder object.
    /// </summary>
    /// <typeparam name="T">The type of values in the dictionary.</typeparam>
    /// <param name="propertyBuilder">The PropertyBuilder object.</param>
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

    /// <summary>
    /// Sets the value comparer for a list property.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the list.</typeparam>
    /// <param name="propertyBuilder">The PropertyBuilder instance.</param>
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