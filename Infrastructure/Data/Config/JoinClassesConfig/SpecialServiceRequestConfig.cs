using Core.PassengerContext.JoinClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config.JoinClassesConfig
{
    public class SpecialServiceRequestConfig : IEntityTypeConfiguration<SpecialServiceRequest>
    {
        public void Configure(EntityTypeBuilder<SpecialServiceRequest> builder)
        {
            builder.HasKey(ssr => new { ssr.PassengerOrItemId, ssr.FlightId, ssr.SSRCodeId });

            builder.HasOne(ssr => ssr.SSRCode)
                .WithMany()
                .HasForeignKey(ssr => ssr.SSRCodeId);

            builder.HasOne(ssr => ssr.PassengerOrItem)
                .WithMany(p => p.SpecialServiceRequests)
                .HasForeignKey(ssr => ssr.PassengerOrItemId);

            builder.HasOne(ssr => ssr.Flight)
                .WithMany(ssr => ssr.SSRList)
                .HasForeignKey(ssr => ssr.FlightId);
        }
    }
}
