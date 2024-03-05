using Core.FlightContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Config
{
    public class OtherFlightConfig : IEntityTypeConfiguration<OtherFlight>
    {
        public void Configure(EntityTypeBuilder<OtherFlight> builder)
        {
            builder.Property(f => f.FlightNumber)
                .HasColumnName("OtherFlightFltNumber");
        }
    }
}
