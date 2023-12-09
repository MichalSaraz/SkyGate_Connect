using Core.SeatingContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Config
{
    public class SeatMapConfig : IEntityTypeConfiguration<SeatMap>
    {
        public void Configure(EntityTypeBuilder<SeatMap> builder)
        {
            builder.HasKey(s => s.Id);            
            builder.Property(sm => sm.FlightClassesSpecification)
                .HasColumnName("FlightClassesSpecification")
                .HasJsonConversion()
                .Metadata
                .SetValueComparer(new ValueComparer<List<FlightClassSpecification>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                ));


            //builder.Property(s => s.FlightClass)
            //    .HasConversion(
            //        v => v.ToString(),
            //        v => (FlightClassEnum)Enum.Parse(typeof(FlightClassEnum), v)
            //    );
            //builder.Property(s => s.SeatPositionsAvailable)
            //    .HasConversion(
            //        v => v.ToString(),
            //        v => (SeatPositionEnum)Enum.Parse(typeof(SeatPositionEnum), v)
            //    );
            //builder.HasMany(a => a.Seats)
            //    .WithOne()
            //    .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
