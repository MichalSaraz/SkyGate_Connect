using Core.BoardingContext;
using Core.FlightContext;
using Core.FlightContext.FlightInfo;
using Core.PassengerContext;
using Core.PassengerContext.Booking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .WithMany(f => f.Flights)
                .HasForeignKey(f => f.AircraftId);
            builder.OwnsOne(a => a.Boarding, add =>
            {
                add.Property(d => d.BoardingStatus)
                    .HasColumnName("BoardingStatus")
                    .HasEnumConversion();
            });            
            builder.Property(f => f.ArrivalDateTime)
                .HasColumnType("timestamp without time zone");
            builder.Property(f => f.FlightStatus)
                .HasEnumConversion();
        }
    }
}
