using Core.FlightContext.FlightInfo;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SeatingContext
{
    public class SeatMap
    {
        public string Id { get; private set; }

        public List<FlightClassSpecification> FlightClassesSpecification { get; private set; } = new List<FlightClassSpecification>();

        public SeatMap(string id, List<FlightClassSpecification> flightClassesSpecification)
        {
            Id = id;
            FlightClassesSpecification = flightClassesSpecification;
        }
    }
}