using Microsoft.AspNetCore.Identity;
using NineCafeProductAppV1.Constants;

namespace NineCafeProductAppV1.Data
{
    public class RoleSeeder
    {
        public static async Task SeedRoleAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if(!await roleManager.RoleExistsAsync(Roles.ADMIN))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.ADMIN));
            }
            if (!await roleManager.RoleExistsAsync(Roles.CUSTOMER))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.CUSTOMER));
            }
        }
    }
}
