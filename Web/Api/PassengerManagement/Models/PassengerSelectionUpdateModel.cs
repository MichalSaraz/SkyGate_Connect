namespace Web.Api.PassengerManagement.Models;

public class PassengerSelectionUpdateModel
{
    public List<Guid> ExistingPassengers { get; set; }
    public List<Guid> PassengersToAdd { get; set; }
}