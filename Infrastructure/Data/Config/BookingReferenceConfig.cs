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
    public class BookingReferenceConfig : IEntityTypeConfiguration<BookingReference>
    {
        public void Configure(EntityTypeBuilder<BookingReference> builder)
        {
            builder.HasKey(b => b.Id);
            builder.HasMany(b => b.LinkedPassengers)
                .WithOne(p => p.PNR)
                .HasForeignKey(p => p.PNRId);
        }
    }
}
