using Core.SeatingContext;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Data.Config
{
    public class SeatMapConfig : IEntityTypeConfiguration<SeatMap>
    {
        public void Configure(EntityTypeBuilder<SeatMap> builder)
        {
            builder.HasKey(sm => sm.Id);
            
            builder.Property(sm => sm.FlightClassesSpecification)
                .HasColumnName("FlightClassesSpecification")
                .HasJsonConversion()
                .SetValueComparerForList();
        }
    }
}
