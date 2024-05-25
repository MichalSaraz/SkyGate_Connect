using Core.PassengerContext.JoinClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config.JoinClassesConfig
{
    public class SpecialServiceRequestConfig : IEntityTypeConfiguration<SpecialServiceRequest>
    {
        public void Configure(EntityTypeBuilder<SpecialServiceRequest> builder)
        {
            builder.HasKey(ssr => new { ssr.PassengerId, ssr.FlightId, ssr.SSRCodeId });

            builder.HasOne(ssr => ssr.SSRCode)
                .WithMany()
                .HasForeignKey(ssr => ssr.SSRCodeId);

            builder.HasOne(ssr => ssr.Passenger)
                .WithMany(p => p.SpecialServiceRequests)
                .HasForeignKey(ssr => ssr.PassengerId);

            builder.HasOne(ssr => ssr.Flight)
                .WithMany(ssr => ssr.SSRList)
                .HasForeignKey(ssr => ssr.FlightId);
        }
    }
}
