using Core.PassengerContext.Booking;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class BookingReferenceConfig : IEntityTypeConfiguration<BookingReference>
    {
        public void Configure(EntityTypeBuilder<BookingReference> builder)
        {
            builder.HasKey(br => br.PNR);
            
            builder.HasMany(br => br.LinkedPassengers)
                .WithOne(pi => pi.PNR)
                .HasForeignKey(pi => pi.PNRId);
            
            builder.Property(br => br.FlightItinerary)
                .HasColumnName("FlightItinerary")
                .HasJsonConversion()
                .SetValueComparerForList();
        }
    }
}
