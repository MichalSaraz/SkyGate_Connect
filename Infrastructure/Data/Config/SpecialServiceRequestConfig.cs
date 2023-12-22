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
    public class SpecialServiceRequestConfig : IEntityTypeConfiguration<SpecialServiceRequest>
    {
        public void Configure(EntityTypeBuilder<SpecialServiceRequest> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(s => s.SSRCode)
                .WithMany()
                .HasForeignKey(s => s.SSRCodeId);

            builder.HasOne(s => s.Passenger)
                .WithMany(p => p.SpecialServiceRequests)
                .HasForeignKey(s => s.PassengerId);

            builder.HasOne(s => s.Flight)
                .WithMany()
                .HasForeignKey(s => s.FlightId);
        }
    }
}
