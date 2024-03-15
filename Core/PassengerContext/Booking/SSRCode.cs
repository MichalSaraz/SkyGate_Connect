using System.ComponentModel.DataAnnotations;

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
