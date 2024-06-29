using System.ComponentModel.DataAnnotations;

namespace Core.FlightContext.FlightInfo
{
    public class AircraftType
    {
        [Key]
        public string AircraftTypeIATACode { get; set; }

        public string ModelName { get; set; }
    }
}
