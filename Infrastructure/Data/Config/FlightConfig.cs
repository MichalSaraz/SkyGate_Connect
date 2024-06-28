using Core.FlightContext;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class FlightConfig : IEntityTypeConfiguration<Flight>
    {
        public void Configure(EntityTypeBuilder<Flight> builder)
        {  
            builder.HasOne(f => f.ScheduledFlight)
                .WithMany()
                .HasForeignKey(f => f.ScheduledFlightId);
            
            builder.HasOne(f => f.Aircraft)
                .WithMany(a => a.Flights)
                .HasForeignKey(f => f.AircraftId);

            builder.Property(f => f.BoardingStatus)
                .HasEnumConversion();

            builder.Property(f => f.FlightStatus)
                .HasEnumConversion();

            builder.HasMany(f => f.Seats)
                .WithOne(s => s.Flight)
                .HasForeignKey(s => s.FlightId);
        }
    }
}
