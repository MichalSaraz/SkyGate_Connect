using Core.FlightContext.FlightInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class AircraftConfig : IEntityTypeConfiguration<Aircraft>
    {
        public void Configure(EntityTypeBuilder<Aircraft> builder)
        {
            builder.HasKey(a => a.RegistrationCode);
            
            builder.HasOne(a => a.Country)
                .WithMany()
                .HasForeignKey(a => a.CountryId);
            
            builder.HasOne(a => a.AircraftType)
                .WithMany()
                .HasForeignKey(a => a.AircraftTypeId);
            
            builder.HasOne(a => a.Airline)
                .WithMany(a => a.Fleet)
                .HasForeignKey(a => a.AirlineId);
            
            builder.HasOne(a => a.SeatMap)
                .WithMany()
                .HasForeignKey(a => a.SeatMapId);
        }
    }
}
