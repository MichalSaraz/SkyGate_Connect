using Core.BaggageContext;
using Core.PassengerContext.Booking;
using Core.PassengerContext.JoinClasses;
using Core.SeatingContext;
using Core.PassengerContext.APIS;

namespace Core.PassengerContext
{
    public class Passenger : PassengerInfo
    {
        public List<APISData> TravelDocuments { get; set; } = new();

        public List<Baggage> PassengerCheckedBags { get; set; } = new();

        public List<PassengerFlight> Flights { get; set; } = new();

        public List<Comment> Comments { get; set; } = new();

        public List<Seat> AssignedSeats { get; set; } = new();

        public List<SpecialServiceRequest> SpecialServiceRequests { get; set; } = new();

        /// <summary>
        /// Maps a <see cref="PassengerInfo"/> object to this <see cref="Passenger"/> object.
        /// </summary>
        /// <param name="passengerInfo">The <see cref="PassengerInfo"/> object to be mapped.</param>
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
            FrequentFlyer = passengerInfo.FrequentFlyer;
            FrequentFlyerId = passengerInfo.FrequentFlyerId;
        }
    }
}
