using Core.FlightContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class BaseFlightConfig : IEntityTypeConfiguration<BaseFlight>
    {
        public void Configure(EntityTypeBuilder<BaseFlight> builder)
        {
            builder.ToTable("Flights")
                .HasDiscriminator<string>("FlightType")
                .HasValue<Flight>("Scheduled")
                .HasValue<OtherFlight>("Other");
            
            builder.HasKey(bf => bf.Id);
            
            builder.HasOne(bf => bf.DestinationFrom)
               .WithMany(d => d.Departures)
               .HasForeignKey(bf => bf.DestinationFromId);
            
            builder.HasOne(bf => bf.DestinationTo)
                .WithMany(d => d.Arrivals)
                .HasForeignKey(bf => bf.DestinationToId);
            
            builder.HasOne(bf => bf.Airline)
                .WithMany()
                .HasForeignKey(bf => bf.AirlineId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Property(bf => bf.DepartureDateTime)
                .HasColumnType("timestamp without time zone");
            
            builder.Property(bf => bf.ArrivalDateTime)
                .HasColumnType("timestamp without time zone");
        }
    }
}
