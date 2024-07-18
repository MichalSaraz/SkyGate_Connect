using System.ComponentModel.DataAnnotations;
using Core.Identity.Enums;
using Microsoft.AspNetCore.Identity;

namespace Core.Identity;

public class AppUser : IdentityUser
{
    public RoleEnum Role { get; set; }
    
    [StringLength(3)]
    public string Station { get; set; }
}