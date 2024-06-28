using Core.PassengerContext;
using Core.PassengerContext.Booking;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class PassengerBookingDetailsConfig : IEntityTypeConfiguration<PassengerBookingDetails>
    {
        public void Configure(EntityTypeBuilder<PassengerBookingDetails> builder)
        {            
            builder.HasKey(pbd => pbd.Id);
            builder.Property(pbd => pbd.Gender)
                .HasEnumConversion();    
            
            builder.HasOne(pbd => pbd.PNR)
                .WithMany(br => br.LinkedPassengers)
                .HasForeignKey(pi => pi.PNRId);

            builder.HasOne(pbd => pbd.PassengerOrItem)
                .WithOne(p => p.BookingDetails)
                .HasForeignKey<BasePassengerOrItem>(p => p.BookingDetailsId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(pbd => pbd.AssociatedPassengerBookingDetails)
                .WithOne()
                .HasForeignKey<PassengerBookingDetails>(p => p.AssociatedPassengerBookingDetailsId)
                .OnDelete(DeleteBehavior.SetNull);
            
            builder.Property(pbd => pbd.BookedClass)
                .HasColumnName("BookedClass")
                .HasJsonConversion()
                .SetValueComparerForDictionary();
            
            builder.Property(pbd => pbd.ReservedSeats)
                .HasColumnName("ReservedSeats")
                .HasJsonConversion()
                .SetValueComparerForDictionary();
            
            builder.Property(pbd => pbd.BookedSSR)
                .HasColumnName("BookedSSR")
                .HasJsonConversion()
                .SetValueComparerForDictionary();
        }
    }
}
