using Core.FlightContext.FlightInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class DestinationConfig : IEntityTypeConfiguration<Destination>
    {
        public void Configure(EntityTypeBuilder<Destination> builder)
        {
            builder.HasKey(d => d.IATAAirportCode);
            
            builder.HasOne(d => d.Country)
                .WithMany()
                .HasForeignKey(d => d.CountryId);
        }
    }
}
