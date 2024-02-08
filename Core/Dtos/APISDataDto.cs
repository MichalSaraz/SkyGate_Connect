using Core.PassengerContext.APIS.Enums;
using Core.PassengerContext.Booking.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace Core.Dtos
{
    public class APISDataDto
    {
        public int Id { get; set; }
        public string DocumentNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string IssueCountry { get; set; }
        public string DocumentType { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}