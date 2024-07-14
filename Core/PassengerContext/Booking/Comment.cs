using System.ComponentModel.DataAnnotations;
using Core.FlightContext.JoinClasses;
using Core.PassengerContext.Booking.Enums;

namespace Core.PassengerContext.Booking
{
    public class Comment
    {
        public Guid Id { get; private set; }

        public BasePassengerOrItem PassengerOrItem { get; }
        public Guid PassengerOrItemId { get; private set; }        

        public PredefinedComment PredefinedComment { get; }
        public string PredefinedCommentId { get; private set; } 
        
        [Required]
        [MaxLength(150)]
        public string Text { get; private set; }

        public CommentTypeEnum CommentType { get; private set; }

        public List<FlightComment> LinkedToFlights { get; private set; } = new();

        // Constructor for adding custom comment
        public Comment(Guid passengerOrItemId, CommentTypeEnum commentType, string text)
        {
            Id = Guid.NewGuid();
            PassengerOrItemId = passengerOrItemId;
            CommentType = commentType;
            Text = text;
        }

        // Constructor for adding predefined comment
        public Comment(Guid passengerOrItemId, string predefinedCommentId, string text, 
            CommentTypeEnum commentType = CommentTypeEnum.Gate)
        {
            Id = Guid.NewGuid();
            PassengerOrItemId = passengerOrItemId;
            PredefinedCommentId = predefinedCommentId;
            Text = text;
            CommentType = commentType;
        }
    }
}
