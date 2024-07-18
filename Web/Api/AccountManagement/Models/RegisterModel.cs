using Core.Identity.Enums;

namespace Web.Api.AccountManagement.Models;

public class RegisterModel
{
    public required string UserName { get; set; }
    public required RoleEnum Role { get; set; }
    public required string Station { get; set; }
    public required string Password { get; set; }
}