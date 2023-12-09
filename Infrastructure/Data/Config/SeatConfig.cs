using Core.PassengerContext.Booking;
using Core.SeatingContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Config
{
    public class SeatConfig : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            builder.HasKey(s => s.Id);
            builder.HasOne(s => s.Passenger)
                .WithMany(p => p.AssignedSeats)
                .HasForeignKey(s => s.PassengerId);
            builder.HasOne(s => s.Flight)
                .WithMany(p => p.Seats)
                .HasForeignKey(s => s.FlightId);
            builder.Property(s => s.Position)
                .HasEnumConversion();
            builder.Property(s => s.SeatType)
                .HasEnumConversion();
            builder.Property(s => s.FlightClass)
                .HasEnumConversion();
            builder.Property(s => s.SeatStatus)
                .HasEnumConversion();
        }
    }
}
