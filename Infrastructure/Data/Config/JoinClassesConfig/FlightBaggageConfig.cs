using Core.FlightContext.JoinClasses;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Data.Config.JoinClassesConfig
{
    public class FlightBaggageConfig : IEntityTypeConfiguration<FlightBaggage>
    {
        public void Configure(EntityTypeBuilder<FlightBaggage> builder)
        {
            builder.HasKey(fb => new { fb.FlightId, fb.BaggageId });

            builder.HasOne(fb => fb.Flight)
                .WithMany(bf => bf.ListOfCheckedBaggage)
                .HasForeignKey(fb => fb.FlightId);

            builder.HasOne(fb => fb.Baggage)
                .WithMany(b => b.Flights)
                .HasForeignKey(fb => fb.BaggageId);
            
            builder.Property(fb => fb.BaggageType)
                .HasEnumConversion();
        }
    }
}
