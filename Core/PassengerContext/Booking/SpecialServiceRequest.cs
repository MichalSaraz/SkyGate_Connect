using Core.PassengerContext.JoinClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PassengerContext.Booking
{
    public class SpecialServiceRequest
    {
        public int Id { get; private set; }

        [Required]
        public SSRCode SSRCode { get; private set; }


        [MaxLength(150)]
        public string FreeText { get; private set; }
        public bool IsFreeTextMandatory { get; private set; }
    }
}
