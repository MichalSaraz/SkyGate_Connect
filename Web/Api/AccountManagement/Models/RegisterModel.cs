using System.ComponentModel.DataAnnotations;
using Core.Identity.Enums;

namespace Web.Api.AccountManagement.Models;

public class RegisterModel
{
    [Required]
    public required string UserName { get; set; }
    
    [Required]
    public required RoleEnum Role { get; set; }
    
    [Required]
    public required string Station { get; set; }
    
    [Required]
    public required string Password { get; set; }
}