using Core.SeatingContext;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class SeatConfig : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            builder.HasKey(s => s.Id);
            
            builder.HasOne(s => s.PassengerOrItem)
                .WithMany(p => p.AssignedSeats)
                .HasForeignKey(s => s.PassengerOrItemId);
            
            builder.HasOne(s => s.Flight)
                .WithMany(f => f.Seats)
                .HasForeignKey(s => s.FlightId);
            
            builder.Property(s => s.Letter)
                .HasEnumConversion();

            builder.Property(s => s.Position)
                .HasEnumConversion();
            
            builder.Property(s => s.SeatType)
                .HasEnumConversion();
            
            builder.Property(s => s.FlightClass)
                .HasEnumConversion();
            
            builder.Property(s => s.SeatStatus)
                .HasEnumConversion();
        }
    }
}
