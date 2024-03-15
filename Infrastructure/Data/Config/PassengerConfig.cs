using Core.PassengerContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Config
{
    public class PassengerConfig : IEntityTypeConfiguration<Passenger>
    {
        public void Configure(EntityTypeBuilder<Passenger> builder)
        {
            builder.HasMany(p => p.AssignedSeats)
                .WithOne(s => s.Passenger)
                .HasForeignKey(s => s.PassengerId);

            builder.HasMany(p => p.SpecialServiceRequests)
                .WithOne(ssr => ssr.Passenger)
                .HasForeignKey(ssr => ssr.PassengerId);

            builder.HasMany(p => p.TravelDocuments)
                .WithOne(ad => ad.Passenger)
                .HasForeignKey(ad => ad.PassengerId);

            builder.HasMany(p => p.PassengerCheckedBags)
                .WithOne(b => b.Passenger)
                .HasForeignKey(b => b.PassengerId);

            builder.HasMany(p => p.Comments)
                .WithOne(c => c.Passenger)
                .HasForeignKey(c => c.PassengerId);            
        }
    }
}
