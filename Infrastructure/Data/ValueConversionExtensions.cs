using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    /// <summary>
    /// Json conversion to database
    /// </summary>
    public static class ValueConversionExtensions
    {
        public static PropertyBuilder<T> HasEnumConversion<T>(this PropertyBuilder<T> propertyBuilder)
        {
            propertyBuilder.HasConversion(
                v => v.ToString(),
                v => (T)Enum.Parse(typeof(T), v));

            return propertyBuilder;
        }

        public static PropertyBuilder<DateTime> HasDateTimeConversion(this PropertyBuilder<DateTime> propertyBuilder)
        {
            propertyBuilder.HasConversion(
                v => v.ToString("ddMMMyyyy", CultureInfo.InvariantCulture),
                v => DateTime.ParseExact(v, "ddMMMyyyy", CultureInfo.InvariantCulture));
            return propertyBuilder;
        }

        public static PropertyBuilder<T> HasJsonEnumArrayConversion<T>(this PropertyBuilder<T> propertyBuilder)
            where T : IList
        {
            // get enum names
            Func<T, List<string>> getNames = v => (from object j in ((IList)v) select j.ToString()).ToList();

            propertyBuilder.HasConversion(
                    v => JsonConvert.SerializeObject(getNames(v),
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                    v => JsonConvert.DeserializeObject<T>(v,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }))
                .HasColumnType("jsonb");

            return propertyBuilder;
        }

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
