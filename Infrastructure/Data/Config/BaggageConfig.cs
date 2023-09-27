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
            builder.OwnsOne(a => a.BaggageTag, add =>
            {
                add.Ignore(d => d.LeadingDigit);
                add.Ignore(d => d.Number);
                add.Property(d => d.TagNumber)
                    .HasColumnName("TagNumber");
            });            
            builder.OwnsOne(a => a.SpecialBag, add =>
            {
                add.Ignore(d => d.Id);
                add.Ignore(d => d.IsDescriptionRequired);
                add.Property(d => d.SpecialBagType)
                    .HasColumnName("SpecialBagType")
                    .HasEnumConversion();
                add.Property(d => d.SpecialBagDescription)
                    .HasColumnName("SpecialBagDescription");
            });
            builder.Property(b => b.BaggageType)
                .HasEnumConversion();
        }
    }
}
