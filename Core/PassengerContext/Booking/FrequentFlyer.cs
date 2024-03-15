using Core.FlightContext.FlightInfo;
using Core.PassengerContext.Booking.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.PassengerContext.Booking
{
    public class FrequentFlyer
    {
        public int Id { get; private set; }

        public string FrequentFlyerNumber
        {
            get => $"{Airline.CarrierCode}{CardNumber}";
            set
            {
                if (value.Length < 2)
                    throw new ArgumentException("Invalid FrequentFlyerNumber");

                Airline.CarrierCode = value[..2];
                CardNumber = value[2..];
            }
        }
        
        public PassengerInfo PassengerInfo { get; private set; }
        public Guid PassengerInfoId { get; private set; }

        [Required]
        public Airline Airline { get; private set; }
        public string AirlineId { get; private set; }
                
        [Required]
        [RegularExpression("^[A-Z0-9]{8,15}$")]
        public string CardNumber { get; private set; }

        [Required]
        public string CardholderName { get; private set; }

        [Required]
        public TierLevelEnum TierLever { get; private set; }

        public long MilesAvailable { get; private set; }        
    }
}
