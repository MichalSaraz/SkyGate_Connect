using Core.PassengerContext.JoinClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Config.JoinClassesConfig
{
    public class PassengerCommentConfig : IEntityTypeConfiguration<PassengerComment>
    {
        public void Configure(EntityTypeBuilder<PassengerComment> builder)
        {
            builder.HasKey(pc => new { pc.PassengerId, pc.CommentId });

            builder.HasOne(pc => pc.Passenger)
                .WithMany(p => p.Comments)
                .HasForeignKey(pc => pc.PassengerId);

            builder.HasOne(pc => pc.Comment)
                .WithMany(c => c.PaxWithComments)
                .HasForeignKey(pc => pc.CommentId);
        }
    }
}
