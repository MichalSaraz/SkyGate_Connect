using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core.PassengerContext.Booking;

namespace Core.PassengerContext.Regulatory
{
    public class APISData
    {
        public int Id { get; private set; }

        [Required]
        [RegularExpression("^[A-Z0-9]{8}$")]
        public string DocumentNumber { get; private set; }


        public Passenger Passenger { get; private set; }
        public int PassengerId { get; private set; }

        public Country Nationality { get; private set; }
        public string NationalityId { get; private set; }

        public Country IssueCountry { get; private set; }
        public string IssueCountryId { get; private set; }

        [Required]
        public DocumentTypeEnum DocumentType { get; private set; }

        [Required]
        public PaxGenderEnum Gender { get; private set; }

        [Required]
        public string FirstName { get; private set; }

        [Required]
        public string LastName { get; private set; }

        [Required]
        public DateTime DateOfBirth { get; private set; }

        [Required]
        public DateTime DateOfIssue { get; private set; }

        [Required]
        public DateTime ExpirationDate { get; private set; }                     
    }
}
