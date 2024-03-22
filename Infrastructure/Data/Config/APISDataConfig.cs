using Core.PassengerContext.APIS;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class APISDataConfig : IEntityTypeConfiguration<APISData>
    {
        public void Configure(EntityTypeBuilder<APISData> builder)
        {
            builder.HasKey(ad => ad.Id);
            
            builder.HasOne(ad => ad.Passenger)
                .WithMany(p => p.TravelDocuments)
                .HasForeignKey(ad => ad.PassengerId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(ad => ad.Nationality)
                .WithMany()
                .HasForeignKey(ad => ad.NationalityId);
            
            builder.HasOne(ad => ad.CountryOfIssue)
                .WithMany()
                .HasForeignKey(ad => ad.CountryOfIssueId);
            
            builder.Property(ad => ad.DocumentType)
                .HasEnumConversion();
            
            builder.Property(ad => ad.Gender)
                .HasEnumConversion();
            
            builder.Property(ad => ad.DateOfBirth)
                .HasDateTimeConversion();
            
            builder.Property(ad => ad.DateOfIssue)
                .HasDateTimeConversion();
            
            builder.Property(ad => ad.ExpirationDate)
                .HasDateTimeConversion();
        }
    }
}
