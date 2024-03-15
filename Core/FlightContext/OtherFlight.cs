namespace Core.FlightContext
{
    public class OtherFlight : BaseFlight
    {
        public string FlightNumber { get; private set; }

        public OtherFlight( 
            string flightNumber,
            DateTime departureDateTime,
            DateTime? arrivalDateTime, 
            string destinationFromId,
            string destinationToId,
            string airlineId)
            : base(
                  departureDateTime,
                  arrivalDateTime,
                  destinationFromId, 
                  destinationToId, 
                  airlineId)
        {
            FlightNumber = flightNumber;
        }
    }
}
