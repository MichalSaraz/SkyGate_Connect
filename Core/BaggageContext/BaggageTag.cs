#nullable enable
using Core.BaggageContext.Enums;
using Core.FlightContext.FlightInfo;

namespace Core.BaggageContext
{
    public class BaggageTag
    {
        public string TagNumber { get; private set; }
        
        public Airline? Airline { get; }
        public string? AirlineId { get; }

        public int LeadingDigit { get; }
        
        public int Number { get; }

        public TagTypeEnum TagType { get; }

        public BaggageTag(string tagNumber)
        {
            TagNumber = tagNumber;
            TagType = TagTypeEnum.Manual;
        }

        public BaggageTag(Airline? airline, int number)
        {
            Airline = airline;
            LeadingDigit = 0;
            Number = number;
            TagNumber = _GetBaggageTagNumber();
            TagType = TagTypeEnum.System;
        }

        private string _GetBaggageTagNumber()
        {
            string airlinePrefix = Airline?.AirlinePrefix ?? "000";
            string carrierCode = Airline?.CarrierCode ?? "XY";
            string number = Number.ToString("D6");

            return $"{LeadingDigit}{airlinePrefix}-{carrierCode}-{number}";
        }
    }
}
