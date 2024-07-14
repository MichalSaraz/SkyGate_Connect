using Core.PassengerContext.JoinClasses;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config.JoinClassesConfig
{
    public class PassengerFlightConfig : IEntityTypeConfiguration<PassengerFlight>
    {
        public void Configure(EntityTypeBuilder<PassengerFlight> builder)
        {
            builder.HasKey(pf => new { pf.PassengerOrItemId, pf.FlightId });
            
            builder.HasOne(pf => pf.PassengerOrItem)
                .WithMany(p => p.Flights)
                .HasForeignKey(pf => pf.PassengerOrItemId);
            
            builder.HasOne(pf => pf.Flight)
                .WithMany(bf => bf.ListOfBookedPassengers)
                .HasForeignKey(pf => pf.FlightId);
            
            builder.Property(pf => pf.BoardingZone)
                .HasNullableEnumConversion();
            
            builder.Property(pf => pf.FlightClass)
                .HasEnumConversion();
            
            builder.Property(pf => pf.AcceptanceStatus)
                .HasEnumConversion();

            builder.Property(pf => pf.NotTravellingReason)
                .HasNullableEnumConversion();
        }
    }
}
