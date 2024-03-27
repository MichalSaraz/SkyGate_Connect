using Core.FlightContext.JoinClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config.JoinClassesConfig
{
    public class FlightCommentConfig : IEntityTypeConfiguration<FlightComment>
    {
        public void Configure(EntityTypeBuilder<FlightComment> builder)
        {
            builder.HasKey(fc => new { fc.CommentId, fc.FlightId });

            builder.HasOne(fc => fc.Comment)
                .WithMany(c => c.LinkedToFlights)
                .HasForeignKey(fc => fc.CommentId);

            builder.HasOne(fc => fc.Flight)
                .WithMany(f => f.Comments)
                .HasForeignKey(fc => fc.FlightId);
        }
    }
}
