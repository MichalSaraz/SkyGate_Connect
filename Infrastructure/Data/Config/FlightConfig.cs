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
            builder.HasKey(f => f.Id);
            builder.HasOne(f => f.Airline)
                .WithMany()
                .HasForeignKey(f => f.AirlineId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(f => f.Aircraft)
                .WithMany()
                .HasForeignKey(f => f.AircraftId);
            builder.OwnsOne(a => a.Boarding, add =>
            {
                add.Property(d => d.BoardingStatus)
                    .HasColumnName("BoardingStatus")
                    .HasEnumConversion();
            });
            builder.Property(f => f.DepartureDateTime)
                .HasDateTimeConversion();
            builder.Property(f => f.ArrivalDateTime)
                .HasDateTimeConversion();
            builder.HasOne(f => f.DepartureAirport)
                .WithMany(f => f.DepartingFlights)
                .HasForeignKey(f => f.DepartureAirportId);
            builder.HasOne(f => f.ArrivalAirport)
                .WithMany()
                .HasForeignKey(f => f.ArrivalAirportId);
            builder.Property(f => f.FlightType)
                .HasEnumConversion();
            builder.Property(f => f.FlightStatus)
                .HasEnumConversion();
            builder.OwnsMany(s => s.Codeshare, fc =>
            {
                fc.Property(fc => fc.CodeshareFlightNumber)
                    .HasColumnName("CodeshareFlightNumber");                   
            });
        }
    }
}
