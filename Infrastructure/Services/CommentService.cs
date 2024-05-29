using Core.FlightContext.JoinClasses;
using Core.Interfaces;
using Core.PassengerContext.Booking;
using Core.PassengerContext.Booking.Enums;

namespace Infrastructure.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPredefinedCommentRepository _predefinedCommentRepository;

        public CommentService(ICommentRepository commentRepository,
            IPredefinedCommentRepository predefinedCommentRepository)
        {
            _commentRepository = commentRepository;
            _predefinedCommentRepository = predefinedCommentRepository;
        }

        public async Task<Comment> AddCommentAsync(Guid id, CommentTypeEnum commentType, string text,
            List<Guid> flightIds, string predefinedCommentId = null)
        {
            Comment comment;

            if (!string.IsNullOrEmpty(predefinedCommentId))
            {
                var predefinedComment =
                    await _predefinedCommentRepository.GetPredefinedCommentByIdAsync(predefinedCommentId);

                if (predefinedComment == null)
                {
                    throw new Exception("Predefined comment not found.");
                }

                var existingComment = await _commentRepository.GetCommentByCriteriaAsync(c =>
                    c.PassengerId == id && c.PredefinedCommentId == predefinedCommentId);

                if (existingComment != null)
                {
                    return null;
                }

                comment = new Comment(id, predefinedCommentId, predefinedComment.Text);
            }
            else
            {
                comment = new Comment(id, commentType, text ?? string.Empty);
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