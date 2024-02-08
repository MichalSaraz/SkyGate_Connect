using Core.PassengerContext.Booking.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace Core.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }

        public Guid PassengerId { get; set; }
        public string CommentType { get; set; }
        public string Text { get; set; }
        public bool? IsMarkedAsRead { get; set; }
    }
}