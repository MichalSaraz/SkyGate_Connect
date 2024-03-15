using Core.BaggageContext;
using Core.BaggageContext.Enums;

namespace Core.FlightContext.JoinClasses
{
    public class FlightBaggage
    {
        public BaseFlight Flight { get; init; }
        public int FlightId { get; private set; }

        public Baggage Baggage { get; init; }
        public Guid BaggageId { get; private set;}

        public BaggageTypeEnum BaggageType { get; init; }
        
        public FlightBaggage(int flightId, Guid baggageId, BaggageTypeEnum baggageType)
        {
            FlightId = flightId;
            BaggageId = baggageId;
            BaggageType = baggageType;
        }
    }
}
