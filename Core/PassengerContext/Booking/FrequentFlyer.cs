using Core.FlightContext.FlightInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PassengerContext.Booking
{
    public class FrequentFlyer
    {
        public int Id { get; private set; }

        public string FrequentFlyerNumber 
        {
            get
            {
                return $"{Airline.CarrierCode}{CardNumber}";
            }
            private set { }
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
