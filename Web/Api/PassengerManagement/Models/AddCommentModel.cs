using System.ComponentModel.DataAnnotations;

namespace Web.Api.PassengerManagement.Models;

public class AddCommentModel
{
    public List<Guid> FlightIds { get; set; } = new();
    
    [Required]
    public required string Text { get; set; }
}