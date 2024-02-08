using Core.BaggageContext;
using Core.BaggageContext.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
