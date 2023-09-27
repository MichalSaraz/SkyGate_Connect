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
    public class AirlineConfig : IEntityTypeConfiguration<Airline>
    {
        public void Configure(EntityTypeBuilder<Airline> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.Country)
                .WithMany()
                .HasForeignKey(p => p.CountryId);
            builder.HasMany(a => a.Fleet)
                .WithOne(a => a.Airline)
                .HasForeignKey(a => a.AirlineId);            
        }
    }
}
