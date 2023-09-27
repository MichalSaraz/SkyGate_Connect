using Core.PassengerContext.Booking;
using Core.PassengerContext.JoinClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Config.JoinClassesConfig
{
    public class PassengerSpecialServiceRequestConfig : IEntityTypeConfiguration<PassengerSpecialServiceRequest>
    {
        public void Configure(EntityTypeBuilder<PassengerSpecialServiceRequest> builder)
        {
            builder.HasKey(ps => new { ps.PassengerId, ps.SpecialServiceRequestId });

            builder.HasOne(ps => ps.Passenger)
                .WithMany(p => p.SSRList)
                .HasForeignKey(ps => ps.PassengerId);

            builder.HasOne(ps => ps.SpecialServiceRequest)
                .WithMany()
                .HasForeignKey(ps => ps.SpecialServiceRequestId);
        }
    }
}
