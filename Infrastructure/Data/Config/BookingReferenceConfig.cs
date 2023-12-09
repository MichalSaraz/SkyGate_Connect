using Core.FlightContext;
using Core.PassengerContext;
using Core.PassengerContext.Booking;
using Core.SeatingContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Config
{
    public class BookingReferenceConfig : IEntityTypeConfiguration<BookingReference>
    {
        public void Configure(EntityTypeBuilder<BookingReference> builder)
        {
            builder.HasKey(b => b.PNR);
            builder.HasMany(b => b.LinkedPassengers)
                .WithOne(p => p.PNR)
                .HasForeignKey(p => p.PNRId);
            builder.Property(sm => sm.FlightItinerary)
                .HasColumnName("FlightItinerary")
                .HasJsonConversion()
                .Metadata
                .SetValueComparer(new ValueComparer<List<KeyValuePair<string, DateTime>>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                ));            
        }
    }
}
