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
                .WithOne(pbd => pbd.Passenger)
                .HasForeignKey<PassengerBookingDetails>(pbd => pbd.PassengerId);            

            builder.HasMany(bpoi => bpoi.TravelDocuments)
                .WithOne(td => td.Passenger)
                .HasForeignKey(td => td.PassengerId);

            builder.HasMany(p => p.Comments)
                .WithOne(c => c.Passenger)
                .HasForeignKey(c => c.PassengerId);

            builder.HasMany(p => p.AssignedSeats)
                .WithOne(s => s.PassengerOrItem)
                .HasForeignKey(s => s.PassengerOrItemId);

            builder.HasMany(bpoi => bpoi.Flights)
                .WithOne(pf => pf.PassengerOrItem)
                .HasForeignKey(pf => pf.PassengerOrItemId);
        }
    }
}
