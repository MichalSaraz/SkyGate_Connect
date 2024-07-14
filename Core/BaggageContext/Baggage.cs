using Core.FlightContext.FlightInfo;
using Core.FlightContext.JoinClasses;
using Core.PassengerContext;
using System.ComponentModel.DataAnnotations;
using Core.HistoryTracking;

namespace Core.BaggageContext
{
    public class Baggage
    {
        public Guid Id { get; private set; }

        [Required]
        public Passenger Passenger { get; init; }
        public Guid PassengerId { get; private set; }

        public BaggageTag BaggageTag { get; set; }

        public SpecialBag SpecialBag { get; set; }

        public Destination FinalDestination { get; init; }
        public string DestinationId { get; private set; }

        [Range(1, 32)]
        public int Weight { get; set; }               

        public List<FlightBaggage> Flights { get; init; } = new();  
        
        public Baggage(Guid passengerId, string destinationId, int weight) 
        {
            Id = Guid.NewGuid();
            PassengerId = passengerId;
            DestinationId = destinationId;
            Weight = weight;
        }
    }
}
