using Core.Identity;
using Core.Identity.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class AppIdentityDbContextSeed
{
    public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
    {
        if (!userManager.Users.Any())
        {
            var user = new AppUser
            {
                UserName = "Admin",
                Role = RoleEnum.Admin,
                Station = "PRG"
            };

            await userManager.CreateAsync(user, "Pa$$w0rd");
        }
    }
}