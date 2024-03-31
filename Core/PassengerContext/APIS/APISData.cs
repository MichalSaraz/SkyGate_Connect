using System.ComponentModel.DataAnnotations;
using Core.PassengerContext.APIS.Enums;
using Core.PassengerContext.Booking.Enums;

namespace Core.PassengerContext.APIS
{
    public class APISData
    {
        public Guid Id { get; set; }        

        public BasePassengerOrItem Passenger { get; set; }
        public Guid? PassengerId { get; set; }

        public Country Nationality { get; set; }
        public string NationalityId { get; set; }

        public Country CountryOfIssue { get; set; }
        public string CountryOfIssueId { get; set; }

        [Required]
        public string DocumentNumber { get; set; }

        [Required]
        public DocumentTypeEnum DocumentType { get; set; }

        [Required]
        public PaxGenderEnum Gender { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public DateTime DateOfIssue { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        public APISData(
            Guid? passengerId,
            string nationalityId,
            string countryOfIssueId,
            string documentNumber, 
            DocumentTypeEnum documentType,
            PaxGenderEnum gender,
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            DateTime dateOfIssue,
            DateTime expirationDate)
        {
            Id = Guid.NewGuid();
            PassengerId = passengerId;
            NationalityId = nationalityId;
            CountryOfIssueId = countryOfIssueId;
            DocumentNumber = documentNumber;
            DocumentType = documentType;
            Gender = gender;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            DateOfIssue = dateOfIssue;
            ExpirationDate = expirationDate;
        }
    }
}
