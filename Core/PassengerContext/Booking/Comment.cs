#nullable enable
using System.ComponentModel.DataAnnotations;
using Core.PassengerContext.Booking.Enums;

namespace Core.PassengerContext.Booking
{
    public class Comment
    {
        public int Id { get; private set; }

        public Passenger? Passenger { get; private set; }
        public Guid PassengerId { get; private set; }

        public PredefinedComment? PredefinedComment { get; private set; }
        public string? PredefinedCommentId { get; private set; } 
        
        [Required]
        [MaxLength(150)]
        public string Text { get; private set; }
        
        public bool? IsMarkedAsRead { get; private set; }

        public CommentTypeEnum CommentType { get; private set; }

        // Constructor for adding custom comment
        public Comment(Guid passengerId, CommentTypeEnum commentType, string text, bool? isMarkedAsRead)
        {
            PassengerId = passengerId;
            CommentType = commentType;
            Text = text;
            IsMarkedAsRead = isMarkedAsRead;
            PredefinedCommentId = null;
        }

        // Constructor for adding predefined comment
        public Comment(Guid passengerId, CommentTypeEnum commentType, bool? isMarkedAsRead, 
            PredefinedComment predefinedComment)
        {
            PassengerId = passengerId;
            CommentType = commentType;
            Text = predefinedComment.Text;
            IsMarkedAsRead = isMarkedAsRead;
            PredefinedCommentId = predefinedComment.Id;
            PredefinedComment = predefinedComment;
        }
    }
}
