using Core.PassengerContext.Booking;

namespace Core.FlightContext.JoinClasses
{
    public class FlightComment
    {
        public Comment Comment { get; }
        public Guid CommentId { get; private set; }
        
        public Flight Flight { get; set; }
        public Guid FlightId { get; private set; }

        public FlightComment(Guid commentId, Guid flightId)
        {
            CommentId = commentId;
            FlightId = flightId;
        }
    }
}
