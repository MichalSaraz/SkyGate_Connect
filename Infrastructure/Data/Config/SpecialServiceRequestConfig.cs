using Core.PassengerContext.Booking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class SpecialServiceRequestConfig : IEntityTypeConfiguration<SpecialServiceRequest>
    {
        public void Configure(EntityTypeBuilder<SpecialServiceRequest> builder)
        {
            builder.HasKey(ssr => ssr.Id);

            builder.Property(ssr => ssr.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(ssr => ssr.SSRCode)
                .WithMany()
                .HasForeignKey(ssr => ssr.SSRCodeId);

            builder.HasOne(ssr => ssr.Passenger)
                .WithMany(p => p.SpecialServiceRequests)
                .HasForeignKey(ssr => ssr.PassengerId);

            builder.HasOne(ssr => ssr.Flight)
                .WithMany()
                .HasForeignKey(ssr => ssr.FlightId);
        }
    }
}
