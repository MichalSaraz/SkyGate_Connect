using Core.PassengerContext.Booking;
using Core.PassengerContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Data.Config
{
    public class PassengerConfig : IEntityTypeConfiguration<Passenger>
    {
        public void Configure(EntityTypeBuilder<Passenger> builder)
        {
            builder.HasMany(p => p.AssignedSeats)
                .WithOne(s => s.Passenger)
                .HasForeignKey(s => s.PassengerId);

            builder.HasMany(p => p.SpecialServiceRequests)
                .WithOne(s => s.Passenger)
                .HasForeignKey(s => s.PassengerId);

            builder.HasMany(p => p.TravelDocuments)
                .WithOne(a => a.Passenger)
                .HasForeignKey(a => a.PassengerId);

            builder.HasMany(p => p.PassengerCheckedBags)
                .WithOne(b => b.Passenger)
                .HasForeignKey(b => b.PassengerId);

            builder.HasMany(c => c.Comments)
                .WithOne(p => p.Passenger)
                .HasForeignKey(p => p.PassengerId);            
        }
    }
}
