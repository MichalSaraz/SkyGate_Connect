using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Infrastructure.Data.Extensions
{
    /// <summary>
    /// Contains extension methods for configuring value conversions for entity properties.
    /// </summary>
    public static class ValueConversionExtensions
    {
        /// <summary>
        /// Adds value conversion for the specified property to support storing and retrieving enum values as strings
        /// in the database.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="propertyBuilder">The property builder.</param>
        /// <returns>The updated property builder.</returns>
        public static PropertyBuilder<T> HasEnumConversion<T>(this PropertyBuilder<T> propertyBuilder)
        {
            propertyBuilder.HasConversion(
                v => v.ToString(),
                v => (T)Enum.Parse(typeof(T), v));

            return propertyBuilder;
        }

        /// <summary>
        /// Adds a value conversion to the property for a nullable enum type.
        /// </summary>
        /// <typeparam name="T">The nullable enum type.</typeparam>
        /// <param name="propertyBuilder">The property builder.</param>
        /// <returns>The property builder.</returns>
        public static PropertyBuilder<T?> HasNullableEnumConversion<T>(
            this PropertyBuilder<T?> propertyBuilder) where T : struct
        {
            propertyBuilder.HasConversion(
                v => v.HasValue ? v.Value.ToString() : null,
                v => (T)Enum.Parse(typeof(T), v));

            return propertyBuilder;
        }

        /// <summary>
        /// Adds a value conversion for converting <see cref="DateTime"/> properties to and from
        /// a specific string format.
        /// </summary>
        /// <param name="propertyBuilder">The <see cref="PropertyBuilder{DateTime}"/> being configured.</param>
        /// <returns>The same <see cref="PropertyBuilder{DateTime}"/> instance so that multiple calls
        /// can be chained.</returns>
        public static PropertyBuilder<DateTime> HasDateTimeConversion(this PropertyBuilder<DateTime> propertyBuilder)
        {
            propertyBuilder.HasConversion(
                v => v.ToString("ddMMMyyyy", CultureInfo.InvariantCulture),
                v => DateTime.ParseExact(v, "ddMMMyyyy", CultureInfo.InvariantCulture));
            return propertyBuilder;
        }

        /// <summary>
        /// Adds JSON conversion for the specified property.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="propertyBuilder">The property builder.</param>
        /// <returns>The property builder with JSON conversion applied.</returns>
        public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder)
        {
            propertyBuilder.HasConversion(
                v => JsonConvert.SerializeObject(v,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeObject<T>(v,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            propertyBuilder.HasColumnType("jsonb");

            return propertyBuilder;
        }
    }
}
