using Core.FlightContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class OtherFlightConfig : IEntityTypeConfiguration<OtherFlight>
    {
        public void Configure(EntityTypeBuilder<OtherFlight> builder)
        {
            builder.Property(of => of.FlightNumber)
                .HasColumnName("OtherFlightFltNumber");
        }
    }
}
