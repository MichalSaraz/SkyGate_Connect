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
        public int Id { get; set; }

        [Required]
        public string DocumentNumber { get; set; }


        public Passenger Passenger { get; set; }
        public Guid PassengerId { get; set; }

        public Country Nationality { get; set; }
        public string NationalityId { get; set; }

        public Country IssueCountry { get; set; }
        public string IssueCountryId { get; set; }

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
    }
}
