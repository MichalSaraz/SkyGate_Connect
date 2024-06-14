namespace Core.Dtos
{
    public class InfantDto : BasePassengerOrItemDto
    {
        public PassengerDetailsDto AssociatedAdultPassenger { get; set; }
    }
}
