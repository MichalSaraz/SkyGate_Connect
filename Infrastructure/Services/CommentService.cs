using Core.FlightContext.JoinClasses;
using Core.Interfaces;
using Core.PassengerContext;
using Core.PassengerContext.Booking;
using Core.PassengerContext.Booking.Enums;

namespace Infrastructure.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPredefinedCommentRepository _predefinedCommentRepository;
        private readonly IBasePassengerOrItemRepository _basePassengerOrItemRepository;
        private readonly IFlightRepository _flightRepository;

        public CommentService(ICommentRepository commentRepository,
            IPredefinedCommentRepository predefinedCommentRepository,
            IBasePassengerOrItemRepository basePassengerOrItemRepository,
            IFlightRepository flightRepository)
        {
            _commentRepository = commentRepository;
            _predefinedCommentRepository = predefinedCommentRepository;
            _basePassengerOrItemRepository = basePassengerOrItemRepository;
            _flightRepository = flightRepository;
        }

        public async Task<Comment> AddCommentAsync(Guid id, CommentTypeEnum commentType, string text,
            List<Guid> flightIds, string predefinedCommentId = null)
        {
            Comment comment;

            var flights = await _flightRepository.GetFlightsByCriteriaAsync(f => flightIds.Contains(f.Id));

            if (flights.Count != flightIds.Count)
            {
                throw new Exception("Flight not found.");
            }

            if (await _basePassengerOrItemRepository.GetBasePassengerOrItemByIdAsync(id) == null)
            {
                throw new Exception("Passenger or item not found.");
            }

            if (!string.IsNullOrEmpty(predefinedCommentId))
            {
                var predefinedComment =
                    await _predefinedCommentRepository.GetPredefinedCommentByIdAsync(predefinedCommentId);

                if (predefinedComment == null)
                {
                    throw new Exception("Predefined comment not found.");
                }
                
                comment = new Comment(id, predefinedCommentId, predefinedComment.Text);              
            }
            else
            {
                if (string.IsNullOrEmpty(text))
                {
                    throw new Exception("Text is required.");
                }
                
                if (await _basePassengerOrItemRepository.GetBasePassengerOrItemByIdAsync(id) is Infant)
                {
                    throw new Exception("Infants cannot have comments.");
                }

                comment = new Comment(id, commentType, text);
            }

            await _commentRepository.AddAsync(comment);

            foreach (var flightId in flightIds)
            {
                var newFlightComment = new FlightComment(comment.Id, flightId);
                comment.LinkedToFlights.Add(newFlightComment);
            }

            await _commentRepository.UpdateAsync(comment);

            return comment;
        }
    }
}