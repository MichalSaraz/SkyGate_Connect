using Core.BaggageContext;
using Core.FlightContext;
using Core.PassengerContext.Booking;
using Core.PassengerContext.JoinClasses;
using Core.PassengerContext.Regulatory;
using Core.SeatingContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PassengerContext
{
    public class Passenger
    {
        public int Id { get; private set; }

        [Required]
        public string FirstName { get; private set; }

        [Required]
        public string LastName { get; private set; }

        [Required]
        public PaxGenderEnum Gender { get; private set; }

        

        [Required]
        public BookingReference PNR { get; private set; }
        public int PNRId { get; private set; }

        public FrequentFlyer FrequentFlyer { get; private set; }
        public int FrequentFlyerId { get; private set; }


        public int BoardingSequenceNumber { get; private set; }

        public BoardingZoneEnum BoardingZone { get; private set; }

        public AcceptanceStatusEnum AcceptanceStatus { get; private set; } = AcceptanceStatusEnum.NotAccepted;


        public List<Seat> SeatAssignments { get; private set; } = new List<Seat>();

        public List<APISData> TravelDocuments { get; private set; } = new List<APISData>();

        public List<Baggage> PassengerCheckedBags { get; private set; } = new List<Baggage>();

        public List<PassengerFlight> Flights { get; private set; } = new List<PassengerFlight>();

        public List<PassengerComment> Comments { get; private set; } = new List<PassengerComment>();
        
        public List<PassengerSpecialServiceRequest> SSRList { get; private set; } = new List<PassengerSpecialServiceRequest>();

        // přidat letenku
    }
}
