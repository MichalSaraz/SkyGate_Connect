using Core.PassengerContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.PassengerContext.Booking;

namespace Infrastructure.Data.Config
{
    public class PassengerConfig : IEntityTypeConfiguration<Passenger>
    {
        public void Configure(EntityTypeBuilder<Passenger> builder)
        {
            builder.HasOne(p => p.FrequentFlyerCard)
                .WithOne(ff => ff.Passenger)
                .HasForeignKey<FrequentFlyer>(ff => ff.PassengerId);

            builder.HasOne(p => p.Infant)
                .WithOne(i => i.AssociatedAdultPassenger)
                .HasForeignKey<Infant>(i => i.AssociatedAdultPassengerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(p => p.PassengerCheckedBags)
                .WithOne(b => b.Passenger)
                .HasForeignKey(b => b.PassengerId);                       
        }
    }
}
