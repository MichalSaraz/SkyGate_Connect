using Core.FlightContext.JoinClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Config.JoinClassesConfig
{
    public class FlightBaggageConfig : IEntityTypeConfiguration<FlightBaggage>
    {
        public void Configure(EntityTypeBuilder<FlightBaggage> builder)
        {
            builder.HasKey(fb => new { fb.FlightId, fb.BaggageId });

            builder.HasOne(fb => fb.Flight)
                .WithMany(f => f.ListOfCheckedBaggage)
                .HasForeignKey(fb => fb.FlightId);

            builder.HasOne(fb => fb.Baggage)
                .WithMany(b => b.Flights)
                .HasForeignKey(fb => fb.BaggageId);
        }
    }
}
