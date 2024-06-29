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
