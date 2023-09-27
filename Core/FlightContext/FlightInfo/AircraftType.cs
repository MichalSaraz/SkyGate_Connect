using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FlightContext.FlightInfo
{
    public class AircraftType
    {
        public int Id { get; private set; }

        public string AircraftTypeIATACode { get; private set; }

        public string ModelName { get; private set; }
    }
}
