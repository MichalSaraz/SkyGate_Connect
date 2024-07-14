using Core.HistoryTracking;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class ActionHistoryConfig : IEntityTypeConfiguration<ActionHistory<object>>
{
    public void Configure(EntityTypeBuilder<ActionHistory<object>> builder)
    {
        builder.HasKey(ah => ah.Id);

        builder.HasOne(e => e.PassengerOrItem)
            .WithMany(p => p.CustomerHistory)
            .HasForeignKey(ah => ah.PassengerOrItemId);

        builder.Property(e => e.Action)
            .HasEnumConversion();

        builder.Property(e => e.Timestamp)
            .HasColumnType("timestamp with time zone");
        builder.Property(e => e.SerializedOldValue)
            .HasColumnType("jsonb");
        
        builder.Property(e => e.SerializedNewValue)
            .HasColumnType("jsonb");
    }
}