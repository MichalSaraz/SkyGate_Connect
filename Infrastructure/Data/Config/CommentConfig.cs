using Core.PassengerContext.Booking;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Data.Config
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);
            
            builder.Property(c => c.CommentType)
                .HasEnumConversion();
            
            builder.HasOne(c => c.PassengerOrItem)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PassengerOrItemId);

            builder.HasOne(c => c.PredefinedComment)
                .WithMany()
                .HasForeignKey(c => c.PredefinedCommentId);
        }
    }
}
