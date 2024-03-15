namespace Core.SeatingContext
{
    public class SeatMap
    {
        public string Id { get; private set; }

        public List<FlightClassSpecification> FlightClassesSpecification { get; private set; }

        public SeatMap(string id, List<FlightClassSpecification> flightClassesSpecification)
        {
            Id = id;
            FlightClassesSpecification = flightClassesSpecification;
        }
    }
}