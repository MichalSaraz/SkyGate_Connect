using Core.PassengerContext;
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
    public class PassengerConfig : IEntityTypeConfiguration<Passenger>
    {
        public void Configure(EntityTypeBuilder<Passenger> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Gender)
                .HasEnumConversion();            
            builder.HasOne(p => p.PNR)
                .WithMany(b => b.LinkedPassengers)
                .HasForeignKey(p => p.PNRId);
            builder.HasOne(p => p.FrequentFlyer)
                .WithOne(f => f.Passenger)
                .HasForeignKey<FrequentFlyer>(f => f.PassengerId);
            builder.Property(p => p.BoardingZone)
                .HasEnumConversion();
            builder.Property(p => p.AcceptanceStatus)
                .HasEnumConversion();

            builder.HasMany(p => p.SeatAssignments)
                .WithOne(s => s.Passenger)
                .HasForeignKey(s => s.PassengerId);

            builder.HasMany(p => p.TravelDocuments)
                .WithOne(a => a.Passenger)
                .HasForeignKey(a => a.PassengerId);

            builder.HasMany(p => p.PassengerCheckedBags)
                .WithOne(b => b.Passenger)
                .HasForeignKey(b => b.PassengerId);
        }
    }
}
