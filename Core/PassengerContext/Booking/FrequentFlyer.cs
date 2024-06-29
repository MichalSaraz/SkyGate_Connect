using Core.FlightContext.FlightInfo;
using Core.PassengerContext.Booking.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.PassengerContext.Booking
{
    public class FrequentFlyer
    {
        public Guid Id { get; private set; }

        public string FrequentFlyerNumber
        {
            get => $"{Airline.CarrierCode}{CardNumber}";
            set
            {
                if (value.Length < 10)
                    throw new ArgumentException("Invalid FrequentFlyerNumber");

                AirlineId = value[..2];
                CardNumber = value[2..];
            }
        }
        
        public Passenger Passenger { get; }
        public Guid PassengerId { get; private set; }

        public Airline Airline { get; }
        public string AirlineId { get; private set; }
                
        [Required]
        [RegularExpression("^[A-Z0-9]{6,15}$")]
        public string CardNumber { get; private set; }

        [Required]
        public string CardholderFirstName { get; private set; }

        [Required]
        public string CardholderLastName { get; private set; }

        [Required]
        public TierLevelEnum TierLever { get; private set; }

        public long MilesAvailable { get; private set; }        
    }
}
