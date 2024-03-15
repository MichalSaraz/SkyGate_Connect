using Core.PassengerContext;
using Core.PassengerContext.Booking;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class PassengerInfoConfig : IEntityTypeConfiguration<PassengerInfo>
    {
        public void Configure(EntityTypeBuilder<PassengerInfo> builder)
        {
            builder.UseTptMappingStrategy();
            
            builder.HasKey(pi => pi.Id);
            builder.Property(pi => pi.Gender)
                .HasEnumConversion();    
            
            builder.HasOne(pi => pi.PNR)
                .WithMany(br => br.LinkedPassengers)
                .HasForeignKey(pi => pi.PNRId);   
            
            builder.HasOne(pi => pi.FrequentFlyer)
                .WithOne(ff => ff.PassengerInfo)
                .HasForeignKey<FrequentFlyer>(ff => ff.PassengerInfoId);
            
            builder.Property(pi => pi.BookedClass)
                .HasColumnName("BookedClass")
                .HasJsonConversion()
                .SetValueComparerForDictionary();
            
            builder.Property(pi => pi.ReservedSeats)
                .HasColumnName("ReservedSeats")
                .HasJsonConversion()
                .SetValueComparerForDictionary();
            
            builder.Property(pi => pi.BookedSSR)
                .HasColumnName("BookedSSR")
                .HasJsonConversion()
                .SetValueComparerForDictionary();
        }
    }
}
