using Core.PassengerContext.Booking.Enums;

namespace Web.Api.PassengerManagement.Models
{
    public class InfantModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public PaxGenderEnum Gender { get; set; }
    }
}