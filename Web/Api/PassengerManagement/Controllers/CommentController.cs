using AutoMapper;
using Core.Dtos;
using Core.HistoryTracking;
using Core.HistoryTracking.Enums;
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
        private readonly ICommentService _commentService;
        private readonly IActionHistoryRepository _actionHistoryRepository;
        private readonly IMapper _mapper;

        public CommentController(
            ICommentRepository commentRepository, 
            ICommentService commentService, 
            IActionHistoryRepository actionHistoryRepository,
            IMapper mapper)
        {
            _commentRepository = commentRepository;
            _commentService = commentService;
            _actionHistoryRepository = actionHistoryRepository;
            _mapper = mapper;
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
        /// <returns>Returns an <see cref="ActionResult{T}"/> containing the result of the add operation. If the comment
        /// was added successfully, returns <see cref="OkResult"/> with the comment data. If the request is invalid,
        /// returns <see cref="BadRequestResult"/> with an error message.</returns>
        [HttpPost("passenger/{id:guid}/add-comment")]
        public async Task<ActionResult<CommentDto>> AddComment(Guid id, CommentTypeEnum commentType,
            [FromBody] JObject data, string? predefineCommentId = null)
        {
            var flightIds = data["flightIds"]?.ToObject<List<Guid>>();
            var text = data["text"]?.ToString();

            if (flightIds == null)
            {
                return BadRequest(new ApiResponse(400, "Flight IDs must be provided."));
            }

            try
            {
                var comment = string.IsNullOrEmpty(predefineCommentId)
                    ? await _commentService.AddCommentAsync(id, commentType, text, flightIds)
                    : await _commentRepository.GetCommentByCriteriaAsync(c =>
                        c.PredefinedCommentId == predefineCommentId && c.PassengerOrItemId == id)
                    ?? await _commentService.AddCommentAsync(id, commentType, text, flightIds, predefineCommentId);
                
                var commentDto = _mapper.Map<CommentDto>(comment);
                
                return Ok(commentDto);
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse(400, e.Message));
            }
        }

        /// <summary>
        /// Deletes comments based on their IDs and flight IDs.
        /// </summary>
        /// <param name="commentIds">A dictionary of flight IDs as keys and a list of comment IDs as values.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /delete-comment
        ///     {
        ///         "58590667-cec5-4a89-a58e-b1572d7086e9": ["17e72928-7c09-48b8-bbe0-a5aa9491eb6c"],
        ///         "99382844-5d43-4051-97a7-07858f76bc7e": ["17e72928-7c09-48b8-bbe0-a5aa9491eb6c", "8ff272ba-58e0-4578-8d8a-71d79b917074"]
        ///     }
        ///
        /// </remarks>
        /// <returns>
        /// Returns an <see cref="ActionResult"/> containing the result of the delete operation.
        /// If any comments were deleted, returns <see cref="NoContentResult"/>.
        /// If no comments were deleted, returns <see cref="OkResult"/>.
        /// </returns>
        [HttpDelete("delete-comment")]
        public async Task<ActionResult> DeleteComment([FromBody] Dictionary<string, List<Guid>> commentIds)
        {
            var commentsToProcess = new Dictionary<Guid, Comment>();
            var deletedComments = new List<Comment>();
            var allCommentIds = commentIds.Values.SelectMany(id => id).Distinct().ToList();

            foreach (var commentId in allCommentIds)
            {
                var comment = await _commentRepository.GetCommentByIdAsync(commentId, true);

                if (comment == null)
                {
                    return NotFound(new ApiResponse(404, $"Comment with Id {commentId} does not exist."));
                }

                commentsToProcess[commentId] = comment;
            }

            foreach (var flight in commentIds.Keys)
            {
                var commentIdsList = commentIds[flight];

                foreach (var commentId in commentIdsList)
                {
                    var comment = commentsToProcess[commentId];

                    if (comment.LinkedToFlights.All(f => f.FlightId != Guid.Parse(flight)))
                    {
                        return BadRequest(new ApiResponse(400,
                            $"Comment with Id {commentId} is not linked to flight with Id {flight}"));
                    }

                    comment.LinkedToFlights.RemoveAll(f => f.FlightId == Guid.Parse(flight));
                }
            }

            foreach (var comment in commentsToProcess.Values)
            {
                ActionHistory<object?> record;
                var commentDto = _mapper.Map<CommentDto>(comment);

                if (comment.LinkedToFlights.Count == 0)
                {
                    record = new ActionHistory<object?>(ActionTypeEnum.Deleted, comment.PassengerOrItemId,
                        nameof(Comment), null, commentDto);

                    await _commentRepository.DeleteAsync(comment);
                    deletedComments.Add(comment);
                }
                else
                {
                    var oldVal = await _commentRepository.GetCommentByIdAsync(comment.Id);

                    record = new ActionHistory<object?>(ActionTypeEnum.Updated, comment.PassengerOrItemId,
                        nameof(Comment), commentDto, _mapper.Map<CommentDto>(oldVal));

                    await _commentRepository.UpdateAsync(comment);
                }

                await _actionHistoryRepository.AddAsync(record);
            }

            if (deletedComments.Any()) return NoContent();
            
            return Ok();
        }

        /// <summary>
        /// Marks a gate comment as read (deletes it).
        /// </summary>
        /// <param name="commentId">The ID of the gate comment to mark as read.</param>
        /// <returns>A <see cref="NoContentResult"/> if the comment was deleted successfully.</returns>
        [HttpDelete("mark-comment-as-read")]
        public async Task<ActionResult> MarkGateCommentAsRead([FromBody] string commentId)
        {
            if (!Guid.TryParse(commentId, out var guidCommentId))
            {
                return BadRequest(new ApiResponse(400, "Invalid comment Id format."));
            }

            var comment = await _commentRepository.GetCommentByCriteriaAsync(c =>
                c.Id == guidCommentId && c.CommentType == CommentTypeEnum.Gate);

            if (comment == null)
            {
                return NotFound(new ApiResponse(404, $"Comment with Id {commentId} and type 'Gate' not found."));
            }
            
            var record = new ActionHistory<object?>(ActionTypeEnum.Deleted, comment.PassengerOrItemId, nameof(Comment), 
                null, _mapper.Map<CommentDto>(comment));

            await _commentRepository.DeleteAsync(comment);
            await _actionHistoryRepository.AddAsync(record);

            return NoContent();
        }
    }
}