using System.ComponentModel.DataAnnotations;

namespace Web.Api.PassengerManagement.Models;

public class SSRCodeModel
{
    [Required]
    public required string SSRCode { get; set; }

    public string? FreeText { get; set; }
}