using Core.FlightContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            builder.HasKey(f => f.Id);
            builder.HasOne(f => f.DestinationFrom)
               .WithMany(f => f.Departures)
               .HasForeignKey(f => f.DestinationFromId);
            builder.HasOne(f => f.DestinationTo)
                .WithMany(f => f.Arrivals)
                .HasForeignKey(f => f.DestinationToId);
            builder.HasOne(f => f.Airline)
                .WithMany()
                .HasForeignKey(f => f.AirlineId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(f => f.DepartureDateTime)
                .HasColumnType("timestamp without time zone");
        }
    }
}
