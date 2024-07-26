using Core.PassengerContext.JoinClasses;

namespace Web.Api.PassengerManagement.Models;

public class SpecialServiceRequestModel
{
    public List<Guid> FlightIds { get; set; } = new();
    public List<SSRCodeModel> SpecialServiceRequests { get; set; } = new();
}