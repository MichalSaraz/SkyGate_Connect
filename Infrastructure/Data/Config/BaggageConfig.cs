using Core.BaggageContext;
using Core.BoardingContext;
using Core.PassengerContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                baggageTag.Ignore(d => d.Airline);
                baggageTag.Ignore(d => d.AirlineId);
                baggageTag.Ignore(d => d.LeadingDigit);
                baggageTag.Property(d => d.Number)
                    .HasDefaultValueSql("nextval('\"BaggageTagsSequence\"')");
                baggageTag.Ignore(d => d.Number);
                baggageTag.Property(d => d.TagNumber)
                    .HasColumnName("TagNumber");
                baggageTag.Property(d => d.TagType)
                    .HasColumnName("TagType")
                    .HasEnumConversion();
            });
            builder.OwnsOne(b => b.SpecialBag, specialBag =>
            {
                specialBag.WithOwner();
                specialBag.Ignore(d => d.Id);
                specialBag.Property(d => d.SpecialBagType)
                    .HasColumnName("SpecialBagType")
                    .HasEnumConversion();
                specialBag.Property(d => d.SpecialBagDescription)
                    .HasColumnName("SpecialBagDescription");
            });
            builder.HasOne(b => b.FinalDestination)
                .WithMany()
                .HasForeignKey(b => b.DestinationId);            
        }
    }
}
