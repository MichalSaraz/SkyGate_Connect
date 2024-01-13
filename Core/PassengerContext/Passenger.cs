using Core.BaggageContext;
using Core.FlightContext;
using Core.PassengerContext.Booking;
using Core.PassengerContext.JoinClasses;
using Core.PassengerContext.Regulatory;
using Core.SeatingContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PassengerContext
{
    public class Passenger : PassengerInfo
    {
        public List<APISData> TravelDocuments { get; set; } = new List<APISData>();

        public List<Baggage> PassengerCheckedBags { get; set; } = new List<Baggage>();

        public List<PassengerFlight> Flights { get; set; } = new List<PassengerFlight>();

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public List<Seat> AssignedSeats { get; set; } = new List<Seat>();

        public List<SpecialServiceRequest> SpecialServiceRequests { get; set; } = new List<SpecialServiceRequest>();

        public Passenger()
        {
            
        }

        // přidat letenku

        public void MapFromPassengerInfo(PassengerInfo passengerInfo)
        {
            Id = new Guid();
            FirstName = passengerInfo.FirstName;
            LastName = passengerInfo.LastName;
            Gender = passengerInfo.Gender;
            PNR = passengerInfo.PNR;
            PNRId = passengerInfo.PNRId;
            Age = passengerInfo.Age;
            BaggageAllowance = passengerInfo.BaggageAllowance;
            PriorityBoarding = passengerInfo.PriorityBoarding;
            FrequentFlyerId = passengerInfo.FrequentFlyerId;
        }
    }
}
