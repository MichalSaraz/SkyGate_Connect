#nullable enable
using System.ComponentModel.DataAnnotations;
using Core.FlightContext.JoinClasses;
using Core.PassengerContext.Booking.Enums;

namespace Core.PassengerContext.Booking
{
    public class Comment
    {
        public Guid Id { get; private set; }

        public BasePassengerOrItem Passenger { get; private set; }
        public Guid PassengerId { get; private set; }        

        public PredefinedComment? PredefinedComment { get; private set; }
        public string? PredefinedCommentId { get; private set; } 
        
        [Required]
        [MaxLength(150)]
        public string Text { get; private set; }

        public CommentTypeEnum CommentType { get; private set; }

        public List<FlightComment> LinkedToFlights { get; private set; } = new();

        // Constructor for adding custom comment
        public Comment(Guid passengerId, CommentTypeEnum commentType, string text)
        {
            Id = Guid.NewGuid();
            PassengerId = passengerId;
            CommentType = commentType;
            Text = text;
        }

        // Constructor for adding predefined comment
        public Comment(Guid passengerId, string predefinedCommentId, string text, 
            CommentTypeEnum commentType = CommentTypeEnum.Gate)
        {
            Id = Guid.NewGuid();
            PassengerId = passengerId;
            PredefinedCommentId = predefinedCommentId;
            Text = text;
            CommentType = commentType;
        }
    }
}
