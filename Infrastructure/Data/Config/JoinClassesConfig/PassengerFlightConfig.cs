using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.FlightContext;
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
        }
    }
}
