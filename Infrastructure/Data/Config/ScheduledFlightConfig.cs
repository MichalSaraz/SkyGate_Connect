using Core.FlightContext;
using Core.SeatingContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data.Config
{
    public class ScheduledFlightConfig : IEntityTypeConfiguration<ScheduledFlight>
    {
        public void Configure(EntityTypeBuilder<ScheduledFlight> builder)
        {
            builder.HasKey(f => f.FlightNumber);
            builder.Property(f => f.DepartureTimes)
                .HasJsonConversion()
                .Metadata
                .SetValueComparer(CreateListValueComparer<KeyValuePair<DayOfWeek, TimeSpan>>());
            builder.Property(f => f.ArrivalTimes)
               .HasJsonConversion()
               .Metadata
               .SetValueComparer(CreateListValueComparer<KeyValuePair<DayOfWeek, TimeSpan>>());
            builder.Property(f => f.FlightDuration)
               .HasJsonConversion()
               .Metadata
               .SetValueComparer(CreateListValueComparer<KeyValuePair<DayOfWeek, TimeSpan>>());            
        }

        private ValueComparer<List<TItem>> CreateListValueComparer<TItem>()
        {
            return new ValueComparer<List<TItem>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()
            );
        }        
    }
}
