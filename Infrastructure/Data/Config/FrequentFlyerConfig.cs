using Core.PassengerContext;
using Core.PassengerContext.Booking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Config
{
    public class FrequentFlyerConfig : IEntityTypeConfiguration<FrequentFlyer>
    {
        public void Configure(EntityTypeBuilder<FrequentFlyer> builder)
        {
            builder.HasKey(f => f.Id);
            builder.HasOne(f => f.Passenger)
                .WithOne(p => p.FrequentFlyer)
                .HasForeignKey<Passenger>(p => p.FrequentFlyerId);
            builder.HasOne(f => f.Airline)
                .WithMany()
                .HasForeignKey(f => f.AirlineId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(f => f.CardNumber)
                .IsUnique();
            builder.Property(f => f.TierLever)
                .HasEnumConversion();
        }
    }
}
