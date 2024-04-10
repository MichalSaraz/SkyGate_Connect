using Core.PassengerContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class InfantConfig : IEntityTypeConfiguration<Infant>
    {
        public void Configure(EntityTypeBuilder<Infant> builder)
        {
            builder.HasOne(i => i.AssociatedAdultPassenger)
                .WithOne(p => p.Infant)
                .HasForeignKey<Passenger>(i => i.InfantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
