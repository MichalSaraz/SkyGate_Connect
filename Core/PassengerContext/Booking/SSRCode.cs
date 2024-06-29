using System.ComponentModel.DataAnnotations;

namespace Core.PassengerContext.Booking
{
    public class SSRCode
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsFreeTextMandatory { get; set; }
    }
}
