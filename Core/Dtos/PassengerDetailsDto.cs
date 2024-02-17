using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class PassengerDetailsDto : PassengerOverviewDto
    {
        public int BaggageAllowance { get; set; }
        public bool PriorityBoarding { get; set; }
        public string FrequentFlyer { get; set; }
        public List<APISDataDto> TravelDocuments { get; set; } = new List<APISDataDto>();
        public List<BaggageOverviewDto> PassengerCheckedBags { get; set; } = new List<BaggageOverviewDto>();
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
        public List<SpecialServiceRequestDto> SpecialServiceRequests { get; set; } = new List<SpecialServiceRequestDto>();
        public List<PassengerFlightDto> ConnectingFlights { get; set; }
        public List<PassengerFlightDto> InboundFlights { get; set; }
        public List<SeatDto> OtherFlightsSeats { get; set; }
    }
}
