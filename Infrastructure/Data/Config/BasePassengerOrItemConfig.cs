using Core.PassengerContext;
using Core.PassengerContext.Booking;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class BasePassengerOrItemConfig : IEntityTypeConfiguration<BasePassengerOrItem>
    {
        public void Configure(EntityTypeBuilder<BasePassengerOrItem> builder)
        {
            builder.ToTable("Passengers")
                .HasDiscriminator<string>("PassengerOrItemType")
                .HasValue<Passenger>("Passenger")
                .HasValue<Infant>("Infant")
                .HasValue<CabinBaggageRequiringSeat>("CBBG")
                .HasValue<ExtraSeat>("EXST");

            builder.HasKey(bpoi => bpoi.Id);

            builder.Property(bpoi => bpoi.Gender)
                .HasEnumConversion();

            builder.HasOne(bpoi => bpoi.BookingDetails)
                .WithOne(pbd => pbd.PassengerOrItem)
                .HasForeignKey<PassengerBookingDetails>(pbd => pbd.PassengerOrItemId);            

            builder.HasMany(bpoi => bpoi.TravelDocuments)
                .WithOne(td => td.Passenger)
                .HasForeignKey(td => td.PassengerId);

            builder.HasMany(bpoi => bpoi.Comments)
                .WithOne(c => c.PassengerOrItem)
                .HasForeignKey(c => c.PassengerOrItemId);

            builder.HasMany(bpoi => bpoi.AssignedSeats)
                .WithOne(s => s.PassengerOrItem)
                .HasForeignKey(s => s.PassengerOrItemId);
            
            builder.HasMany(bpoi => bpoi.CustomerHistory)
                .WithOne(ah => ah.PassengerOrItem)
                .HasForeignKey(ah => ah.PassengerOrItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
