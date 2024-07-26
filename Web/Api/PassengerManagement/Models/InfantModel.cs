using System.ComponentModel.DataAnnotations;
using Core.PassengerContext.Booking.Enums;

namespace Web.Api.PassengerManagement.Models
{
    public class InfantModel
    {
        [Required]
        public required string FirstName { get; set; }
        
        [Required]
        public required string LastName { get; set; }
        
        [Required]
        public PaxGenderEnum Gender { get; set; }
        
        [Required]
        public required string FreeText { get; set; }
    }
}