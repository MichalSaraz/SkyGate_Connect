using System.ComponentModel.DataAnnotations;
using Core.PassengerContext.APIS.Enums;
using Core.PassengerContext.Booking.Enums;

namespace Web.Api.PassengerManagement.Models
{
    public class APISDataModel
    {
        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        public PaxGenderEnum Gender { get; set; }

        [Required]
        [RegularExpression(@"^([1-9]|[12][0-9]|3[01])(?i)(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)([0-9]{4})$",
            ErrorMessage = "Date must be in the format dMMMyyyy or DDMMMyyyy")]
        public required string DateOfBirth { get; set; }

        [Required]
        public required string DocumentNumber { get; set; }

        [Required]
        public DocumentTypeEnum DocumentType { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z]{3}$", ErrorMessage = "Country must be in the format XXX")]
        public required string CountryOfIssue { get; set; }

        [Required]
        [RegularExpression(@"^([1-9]|[12][0-9]|3[01])(?i)(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)([0-9]{4})$",
            ErrorMessage = "Date must be in the format dMMMyyyy or DDMMMyyyy")]
        public required string DateOfIssue { get; set; }

        [Required]
        [RegularExpression(@"^([1-9]|[12][0-9]|3[01])(?i)(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)([0-9]{4})$",
            ErrorMessage = "Date must be in the format dMMMyyyy or DDMMMyyyy")]
        public required string ExpirationDate { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z]{3}$", ErrorMessage = "Country must be in the format XXX")]
        public required string Nationality { get; set; }
    }
}
