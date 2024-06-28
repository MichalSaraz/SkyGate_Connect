using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class BoardingDto
    {
        public string BoardingStatus { get; init; }
        public int NumberOfGateComments { get; init; }
        public int NumberOfPassengersWithSpecialServiceRequests { get; init; }
        public int NumberOfPassengersWithPriorityBoarding { get; init; }
        public int BoardedPassengers { get; init; }
        public Dictionary<char, int> NotBoardedPassengersByZones { get; init; }
    }
}
