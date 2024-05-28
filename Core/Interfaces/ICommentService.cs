using Core.PassengerContext.Booking;
using Core.PassengerContext.Booking.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICommentService
    {
        Task<Comment> AddCommentAsync(Guid id, CommentTypeEnum commentType, string text, List<Guid> flightIds, string predefinedCommentId = null);
    }
}
