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
            
            builder.HasOne(ff => ff.Passenger)
                .WithOne(p => p.FrequentFlyerCard)
                .HasForeignKey<Passenger>(p => p.FrequentFlyerCardId);
            
            builder.HasOne(ff => ff.Airline)
                .WithMany()
                .HasForeignKey(ff => ff.AirlineId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Ignore(ff => ff.FrequentFlyerNumber);
            
            builder.Property(ff => ff.TierLever)
                .HasEnumConversion();
        }
    }
}
