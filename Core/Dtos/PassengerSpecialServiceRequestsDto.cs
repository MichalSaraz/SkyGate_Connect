namespace Core.Dtos
{
    public class PassengerSpecialServiceRequestsDto : BasePassengerOrItemDto
    {
        public List<SpecialServiceRequestDto> SpecialServiceRequests { get; set; } = new();
    }
}
