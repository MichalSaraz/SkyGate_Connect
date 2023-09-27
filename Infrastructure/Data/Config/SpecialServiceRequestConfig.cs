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
            builder.HasKey(s => s.Id);
            builder.OwnsOne(s => s.SSRCode, ad =>
            {
                ad.Property(s => s.Code).HasColumnName("Code");
                ad.Property(s => s.Description).HasColumnName("Description");
            });
        }
    }
}
