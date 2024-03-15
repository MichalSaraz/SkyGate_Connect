using Core.PassengerContext;
using Core.PassengerContext.Booking;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class FrequentFlyerConfig : IEntityTypeConfiguration<FrequentFlyer>
    {
        public void Configure(EntityTypeBuilder<FrequentFlyer> builder)
        {
            builder.HasKey(ff => ff.Id);
            
            builder.HasOne(ff => ff.PassengerInfo)
                .WithOne(pi => pi.FrequentFlyer)
                .HasForeignKey<PassengerInfo>(pi => pi.FrequentFlyerId);
            
            builder.HasOne(ff => ff.Airline)
                .WithMany()
                .HasForeignKey(ff => ff.AirlineId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasIndex(ff => ff.CardNumber)
                .IsUnique();
            
            builder.Property(ff => ff.TierLever)
                .HasEnumConversion();
        }
    }
}
