using Core.FlightContext.FlightInfo;
using Core.PassengerContext.Booking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
