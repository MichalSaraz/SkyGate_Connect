namespace Core.Dtos
{
    public class PassengerDetailsDto : PassengerOrItemOverviewDto
    {
        public int BaggageAllowance { get; init; }
        public bool PriorityBoarding { get; init; }
        public string FrequentFlyerNumber { get; init; }
        public InfantOverviewDto Infant { get; init; }
        public List<APISDataDto> TravelDocuments { get; init; } = new();
        public List<BaggageOverviewDto> PassengerCheckedBags { get; init; } = new();
        public List<CommentDto> Comments { get; init; } = new();
        public List<SpecialServiceRequestDto> SpecialServiceRequests { get; set; } = new();
    }
}
