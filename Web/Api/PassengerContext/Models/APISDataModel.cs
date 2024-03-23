using Core.PassengerContext.APIS.Enums;
using Core.PassengerContext.Booking.Enums;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.PassengerContext.Models
{
    public class APISDataModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public PaxGenderEnum Gender { get; set; }

        [Required]
        [RegularExpression(@"^([1-9]|[12][0-9]|3[01])(?i)(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)([0-9]{4})$",
            ErrorMessage = "Date must be in the format dMMMyyyy or DDMMMyyyy")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string DocumentNumber { get; set; }

        [Required]
        public DocumentTypeEnum DocumentType { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z]{3}$", ErrorMessage = "Country must be in the format XXX")]
        public string CountryOfIssue { get; set; }

        [Required]
        [RegularExpression(@"^([1-9]|[12][0-9]|3[01])(?i)(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)([0-9]{4})$",
            ErrorMessage = "Date must be in the format dMMMyyyy or DDMMMyyyy")]
        public DateTime DateOfIssue { get; set; }

        [Required]
        [RegularExpression(@"^([1-9]|[12][0-9]|3[01])(?i)(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)([0-9]{4})$",
            ErrorMessage = "Date must be in the format dMMMyyyy or DDMMMyyyy")]
        public DateTime ExpirationDate { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z]{3}$", ErrorMessage = "Country must be in the format XXX")]
        public string Nationality { get; set; }
    }
}
