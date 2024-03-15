using Core.FlightContext.FlightInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class AirlineConfig : IEntityTypeConfiguration<Airline>
    {
        public void Configure(EntityTypeBuilder<Airline> builder)
        {
            builder.HasKey(a => a.CarrierCode);
            
            builder.HasOne(a => a.Country)
                .WithMany()
                .HasForeignKey(a => a.CountryId);
            
            builder.HasMany(a => a.Fleet)
                .WithOne(a => a.Airline)
                .HasForeignKey(a => a.AirlineId);            
        }
    }
}
