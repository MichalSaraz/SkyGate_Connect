using Core.PassengerContext.Booking.Enums;

namespace Web.Api.PassengerManagement.Models
{
    public class InfantModel
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public PaxGenderEnum Gender { get; set; }
        public required string FreeText { get; set; }
    }
}