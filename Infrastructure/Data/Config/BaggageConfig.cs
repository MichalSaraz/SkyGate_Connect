using Core.BaggageContext;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class BaggageConfig : IEntityTypeConfiguration<Baggage>
    {
        public void Configure(EntityTypeBuilder<Baggage> builder)
        {
            builder.HasKey(b => b.Id);
            
            builder.HasOne(b => b.Passenger)
                .WithMany(p => p.PassengerCheckedBags)
                .HasForeignKey(b => b.PassengerId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.OwnsOne(b => b.BaggageTag, baggageTag =>
            {
                baggageTag.WithOwner();
                baggageTag.Ignore(bt => bt.Airline);
                baggageTag.Ignore(bt => bt.AirlineId);
                baggageTag.Ignore(bt => bt.LeadingDigit);
                baggageTag.Property(bt => bt.Number)
                    .HasDefaultValueSql("nextval('\"BaggageTagsSequence\"')");
                baggageTag.Ignore(bt => bt.Number);
                baggageTag.Property(bt => bt.TagNumber)
                    .HasColumnName("TagNumber");
                baggageTag.Property(bt => bt.TagType)
                    .HasColumnName("TagType")
                    .HasEnumConversion();
            });
            
            builder.OwnsOne(b => b.SpecialBag, specialBag =>
            {
                specialBag.WithOwner();
                specialBag.Property(sb => sb.SpecialBagType)
                    .HasColumnName("SpecialBagType")
                    .HasEnumConversion();
                specialBag.Property(sb => sb.SpecialBagDescription)
                    .HasColumnName("SpecialBagDescription");
            });
            
            builder.HasOne(b => b.FinalDestination)
                .WithMany()
                .HasForeignKey(b => b.DestinationId);            
        }
    }
}
