//using Core.SeatingContext;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.ChangeTracking;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure.Data.Config
//{
//    public class FlightClassSpecificationConfig : IEntityTypeConfiguration<FlightClassSpecification>
//    {
//        public void Configure(EntityTypeBuilder<FlightClassSpecification> builder)
//        {
//            builder.HasKey(fc => fc.Id);
//            builder.Property(fc => fc.FlightClass)
//                    .HasEnumConversion();
//            builder.Property(fc => fc.SeatPositionsAvailable)
//                .HasColumnName("SeatPositionsAvailable")
//                .HasJsonConversion()
//                .Metadata
//                .SetValueComparer(CreateListValueComparer<string>());
//            builder.Property(fc => fc.ExitRowSeats)
//                .HasColumnName("ExitRowSeats")
//                .HasJsonConversion()
//                .Metadata
//                .SetValueComparer(CreateListValueComparer<string>());
//            builder.Property(fc => fc.BassinetSeats)
//                .HasColumnName("BassinetSeats")
//                .HasJsonConversion()
//                .Metadata
//                .SetValueComparer(CreateListValueComparer<string>());
//            builder.Property(fc => fc.NotExistingSeats)
//                .HasColumnName("NotExistingSeats")
//                .HasJsonConversion()
//                .Metadata
//                .SetValueComparer(CreateListValueComparer<string>());
//            builder.Property(fc => fc.RowRange)
//                .HasColumnName("RowRange")
//                .HasJsonConversion()
//                .Metadata
//                .SetValueComparer(CreateListValueComparer<int>());
//        }

//        private ValueComparer<List<TItem>> CreateListValueComparer<TItem>()
//        {
//            return new ValueComparer<List<TItem>>(
//                (c1, c2) => c1.SequenceEqual(c2), // Porovnání kolekcí
//                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), // Hash funkce pro kolekci
//                c => c.ToList() // Kopie kolekce
//            );
//        }
//    }
//}
