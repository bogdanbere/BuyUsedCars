using BuyUsedCars.Models;
using Microsoft.AspNetCore.Identity;

namespace BuyUsedCars.Data;

public class Seed
{
    public static async void SeedAdmin(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if(!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if(!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
            string adminEmail = "admin@usedcars.com";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdminUser = new User()
                {
                    UserName = "bogdanbere",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    PhoneNumber = "1234567",
                    PhoneNumberConfirmed = true
                };
                await userManager.CreateAsync(newAdminUser, "Password123!");
                await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
            }

            string appUserEmail = "user@test.com";

            var appUser = await userManager.FindByEmailAsync(appUserEmail);
            if (appUser == null)
            {
                var newAppUser = new User()
                {
                    UserName = "user",
                    Email = appUserEmail,
                    EmailConfirmed = true,
                    PhoneNumber = "1023345",
                    PhoneNumberConfirmed = true
                };
                await userManager.CreateAsync(newAppUser, "Password123!");
                await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
            }
        }
    }
}