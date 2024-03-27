using Core.PassengerContext.Booking;

namespace Core.FlightContext.JoinClasses
{
    public class FlightComment
    {
        public Comment Comment { get; private set; }
        public Guid CommentId { get; private set; }
        
        public Flight Flight { get; private set; }
        public int FlightId { get; private set; }

        public FlightComment(Guid commentId, int flightId)
        {
            CommentId = commentId;
            FlightId = flightId;
        }
    }
}
