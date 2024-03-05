using Core.PassengerContext.JoinClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config.JoinClassesConfig
{
    public class PassengerFlightConfig : IEntityTypeConfiguration<PassengerFlight>
    {
        public void Configure(EntityTypeBuilder<PassengerFlight> builder)
        {
            builder.HasKey(pf => new { pf.PassengerId, pf.FlightId });

            builder.HasOne(pf => pf.Passenger)
                .WithMany(p => p.Flights)
                .HasForeignKey(pf => pf.PassengerId);            

            builder.HasOne(pf => pf.Flight)
                .WithMany(f => f.ListOfBookedPassengers)
                .HasForeignKey(pf => pf.FlightId);

            builder.Property(p => p.BoardingZone)
                .HasNullableEnumConversion();

            builder.Property(p => p.FlightClass)
                .HasEnumConversion();

            builder.Property(p => p.AcceptanceStatus)
                .HasEnumConversion();
        }
    }
}
