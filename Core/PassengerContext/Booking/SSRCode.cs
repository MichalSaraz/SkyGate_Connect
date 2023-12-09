using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PassengerContext.Booking
{
    public class SSRCode
    {
        [Key]
        public string Code { get; private set; }
        public string Description { get; private set; }
        public bool IsFreeTextMandatory { get; private set; }
    }
}
