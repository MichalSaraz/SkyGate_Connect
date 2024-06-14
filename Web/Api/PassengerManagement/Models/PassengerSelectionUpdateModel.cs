namespace Web.Api.PassengerManagement.Models;

public class PassengerSelectionUpdateModel
{
    public List<Guid> ExistingPassengers { get; set; } = new();
    public List<Guid> PassengersToAdd { get; set; } = new();
}