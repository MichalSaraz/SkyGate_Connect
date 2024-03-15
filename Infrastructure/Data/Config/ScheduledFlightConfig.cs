using Core.FlightContext;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ScheduledFlightConfig : IEntityTypeConfiguration<ScheduledFlight>
    {
        public void Configure(EntityTypeBuilder<ScheduledFlight> builder)
        {
            builder.HasKey(sf => sf.FlightNumber);
            
            builder.Property(sf => sf.DepartureTimes)
                .HasJsonConversion()
                .SetValueComparerForList();
            
            builder.Property(sf => sf.ArrivalTimes)
               .HasJsonConversion()
               .SetValueComparerForList();
            
            builder.Property(sf => sf.FlightDuration)
               .HasJsonConversion()
               .SetValueComparerForList();
        }       
    }
}
