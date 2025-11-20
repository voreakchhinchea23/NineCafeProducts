using Microsoft.AspNetCore.Identity;
using NineCafeProductAppV1.Constants;

namespace NineCafeProductAppV1.Data
{
    public class UserSeeder
    {
        public static async Task UserSeederAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await CreateUserWithRole(userManager, "admin@ninecafe.com", "Admin123$", Roles.ADMIN);
            await CreateUserWithRole(userManager, "user@ninecafe.com", "Abc123$", Roles.CUSTOMER);
        }

        private static async Task CreateUserWithRole(
            UserManager<IdentityUser> userManager, 
            string email, 
            string password, 
            string role)
        {
            if(await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser
                {
                    Email = email,
                    EmailConfirmed = true,
                    UserName = email
                };

                var result = await userManager.CreateAsync(user, password);

                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
                else
                {
                    throw new Exception($"Failed creating user with email {user.Email}. Error: {string.Join(",", result.Errors)}");
                }
            }
        }
    }
}
