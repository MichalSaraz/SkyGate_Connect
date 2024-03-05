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
    public class PassengerInfoConfig : IEntityTypeConfiguration<PassengerInfo>
    {
        public void Configure(EntityTypeBuilder<PassengerInfo> builder)
        {
            builder.UseTptMappingStrategy();
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Gender)
                .HasEnumConversion();            
            builder.HasOne(p => p.PNR)
                .WithMany(b => b.LinkedPassengers)
                .HasForeignKey(p => p.PNRId);            
            builder.HasOne(p => p.FrequentFlyer)
                .WithOne(f => f.PassengerInfo)
                .HasForeignKey<FrequentFlyer>(f => f.PassengerInfoId);
            builder.Property(sm => sm.ReservedSeats)
                .HasColumnName("ReservedSeats")
                .HasJsonConversion()
                .Metadata
                .SetValueComparer(
                new ValueComparer<Dictionary<string, string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.Key.GetHashCode(), v.Value.GetHashCode())),
                    c => c.ToDictionary(x => x.Key, x => x.Value)
                )
            );
            builder.Property(sm => sm.BookedSSR)
                .HasColumnName("BookedSSR")
                .HasJsonConversion()
                .Metadata
                .SetValueComparer(
                new ValueComparer<Dictionary<string, List<string>>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.Key.GetHashCode(), v.Value.GetHashCode())),
                    c => c.ToDictionary(x => x.Key, x => x.Value)
                )
            );
        }
    }
}
