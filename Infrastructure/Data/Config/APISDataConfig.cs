using Core.PassengerContext.Booking;
using Core.PassengerContext.Regulatory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Config
{
    public class APISDataConfig : IEntityTypeConfiguration<APISData>
    {
        public void Configure(EntityTypeBuilder<APISData> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasOne(a => a.Passenger)
                .WithMany(p => p.TravelDocuments)
                .HasForeignKey(a => a.PassengerId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.Nationality)
                .WithMany()
                .HasForeignKey(a => a.NationalityId);
            builder.HasOne(a => a.IssueCountry)
                .WithMany()
                .HasForeignKey(a => a.IssueCountryId);
            builder.OwnsOne(a => a.DocumentType, add =>
            {
                add.Ignore(d => d.Id);
                add.Property(d => d.Type).HasColumnName("DocumentType");
            });
            builder.Property(a => a.Gender)
                .HasEnumConversion();
            builder.Property(a => a.DateOfBirth)
                .HasDateTimeConversion();
            builder.Property(a => a.DateOfIssue)
                .HasDateTimeConversion();
            builder.Property(a => a.ExpirationDate)
                .HasDateTimeConversion();
        }
    }
}
