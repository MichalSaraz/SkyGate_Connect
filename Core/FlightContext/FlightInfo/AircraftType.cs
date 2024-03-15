using System.ComponentModel.DataAnnotations;

namespace Core.FlightContext.FlightInfo
{
    public class AircraftType
    {
        [Key]
        public string AircraftTypeIATACode { get; private set; }

        public string ModelName { get; private set; }
    }
}
