using Core.FlightContext.JoinClasses;
using Core.Interfaces;
using Core.PassengerContext.Booking;
using Core.PassengerContext.Booking.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Web.Errors;

namespace Web.Api.PassengerManagement.Controllers
{
    [ApiController]
    [Route("passenger-management/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPredefinedCommentRepository _predefinedCommentRepository;

        public CommentController(
            ICommentRepository commentRepository,
            IPredefinedCommentRepository predefinedCommentRepository)
        {
            _commentRepository = commentRepository;
            _predefinedCommentRepository = predefinedCommentRepository;
        }

        /// <summary>
        /// Adds a new comment for a passenger.
        /// </summary>
        /// <param name="id">The ID of the passenger.</param>
        /// <param name="commentType">The type of the comment (Checkin or Gate).</param>
        /// <param name="data">The data object containing the flight IDs and text.</param>
        /// <param name="predefineCommentId">Optional. The ID of a predefined comment to add.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /passenger/3F2504E0-4F89-41D3-9A0C-0305E82C3301/add-comment
        ///     {
        ///         "flightIds": [
        ///             "58590667-cec5-4a89-a58e-b1572d7086e9",
        ///             "99382844-5d43-4051-97a7-07858f76bc7e"
        ///         ],
        ///         "text": "Passenger informed about seat change"
        ///     }
        ///
        /// </remarks>
        /// <returns>An ActionResult of type Comment.</returns>
        [HttpPost("passenger/{id:guid}/add-comment")]
        public async Task<ActionResult<Comment>> AddComment(Guid id, CommentTypeEnum commentType,
            [FromBody] JObject data, string predefineCommentId = null)
        {
            var flightIds = data["flightIds"]?.ToObject<List<Guid>>();
            var text = data["text"]?.ToString();

            Comment comment;

            if (!string.IsNullOrEmpty(predefineCommentId))
            {
                var predefinedComment =
                    await _predefinedCommentRepository.GetPredefinedCommentByIdAsync(predefineCommentId);

                if (predefinedComment == null)
                {
                    return BadRequest(new ApiResponse(400, "Predefined comment not found."));
                }

                var existingComment = await _commentRepository.GetCommentByCriteriaAsync(c =>
                    c.PassengerId == id && c.PredefinedCommentId == predefineCommentId);

                if (existingComment != null)
                {
                    return BadRequest(new ApiResponse(400, "Predefined comment already exists."));
                }

                comment = new Comment(id, predefineCommentId, predefinedComment.Text);
            }
            else
            {
                comment = new Comment(id, commentType, text ?? string.Empty);
            }

            await _commentRepository.AddAsync(comment);

            if (flightIds == null)
            {
                return BadRequest(new ApiResponse(400, "Flight IDs must be provided."));
            }

            foreach (var flightId in flightIds)
            {
                var newFlightComment = new FlightComment(comment.Id, flightId);
                comment.LinkedToFlights.Add(newFlightComment);
            }

            await _commentRepository.UpdateAsync(comment);

            return Ok();
        }

        /// <summary>
        /// Deletes comments based on their IDs and flight IDs.
        /// </summary>
        /// <param name="commentIds">A dictionary of flight IDs as keys and a list of comment IDs as values.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /delete-comment
        ///     "58590667-cec5-4a89-a58e-b1572d7086e9": ["17e72928-7c09-48b8-bbe0-a5aa9491eb6c"],
        ///     "99382844-5d43-4051-97a7-07858f76bc7e": ["17e72928-7c09-48b8-bbe0-a5aa9491eb6c", "8ff272ba-58e0-4578-8d8a-71d79b917074"]
        ///
        /// </remarks>
        /// <returns>
        /// Returns an <see cref="ActionResult"/> containing the result of the delete operation.
        /// If any comments were deleted, returns <see cref="NoContentResult"/>.
        /// If no comments were deleted, returns <see cref="OkResult"/>.
        /// </returns>
        [HttpDelete("delete-comment")]
        public async Task<ActionResult<Comment>> DeleteComment([FromBody] Dictionary<string, List<Guid>> commentIds)
        {
            var commentsToDelete = new HashSet<Comment>();

            foreach (var flight in commentIds.Keys)
            {
                var commentIdsList = commentIds[flight];

                foreach (var commentId in commentIdsList)
                {
                    var comment = await _commentRepository.GetCommentByIdAsync(commentId);

                    if (comment == null)
                    {
                        return BadRequest(new ApiResponse(400, $"Comment with ID {commentId} does not exist."));
                    }

                    if (comment.LinkedToFlights.All(f => f.FlightId != Guid.Parse(flight)) &&
                        comment.LinkedToFlights.Count > 0)
                    {
                        return BadRequest(new ApiResponse(400,
                            $"Comment with ID {commentId} is not linked to flight with ID {flight}"));
                    }

                    comment.LinkedToFlights.RemoveAll(f => f.FlightId == Guid.Parse(flight));

                    if (commentsToDelete.All(c => c.Id != commentId) && comment.LinkedToFlights.Count == 0)
                    {
                        commentsToDelete.Add(comment);
                    }
                }
            }

            foreach (var comment in commentsToDelete)
            {
                if (comment.LinkedToFlights.Count == 0)
                {
                    await _commentRepository.DeleteAsync(comment);
                }
                else
                {
                    await _commentRepository.UpdateAsync(comment);
                }
            }

            return commentsToDelete.Any() ? NoContent() : Ok();
        }

        /// <summary>
        /// Marks a gate comment as read (deletes it).
        /// </summary>
        /// <param name="commentId">The ID of the gate comment to mark as read.</param>
        /// <returns>Returns an ActionResult of type Comment. If the gate comment with the specified ID does not exist,
        /// a BadRequest response with an ApiResponse message will be returned. Otherwise, a NoContent response will be
        /// returned.</returns>
        [HttpDelete("mark-comment-as-read")]
        public async Task<ActionResult<Comment>> MarkGateCommentAsRead([FromBody] Guid commentId)
        {
            var comment = await _commentRepository.GetCommentByCriteriaAsync(c =>
                c.Id == commentId && c.CommentType == CommentTypeEnum.Gate);

            if (comment == null)
            {
                return BadRequest(new ApiResponse(400, $"Gate comment with ID {commentId} does not exist."));
            }

            await _commentRepository.DeleteAsync(comment);

            return NoContent();
        }
    }
}