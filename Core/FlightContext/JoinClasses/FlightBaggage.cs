using Core.BaggageContext;
using Core.BaggageContext.Enums;

namespace Core.FlightContext.JoinClasses
{
    public class FlightBaggage
    {
        public Flight Flight { get; set; }
        public int FlightId { get; set; }

        public Baggage Baggage { get; set; }
        public Guid BaggageId { get; set; }

        public BaggageTypeEnum BaggageType { get; set; }
    }
}
