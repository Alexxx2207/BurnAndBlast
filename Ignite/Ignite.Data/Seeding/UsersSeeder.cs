using Ignite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Data.Seeding
{
    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedUserAsync(userManager, "as@as.as", "as@as.as", "Mario", "Avramov", "123456", role: GlobalConstants.GlobalConstants.AdministratorRoleName);
            await SeedUserAsync(userManager, "tisho@tisho.tisho", "tisho@tisho.tisho", "Tihomir", "Milev", "123456", role: GlobalConstants.GlobalConstants.TrainerRoleName);
            await SeedUserAsync(userManager, "misha@misha.misha", "misha@misha.misha", "Misha", "Sergeeva", "123456", role: GlobalConstants.GlobalConstants.TrainerRoleName);
            await SeedUserAsync(userManager, "alex@alex.alex", "alex@alex.alex", "Alexander", "Nikolov", "123456", role: GlobalConstants.GlobalConstants.TrainerRoleName);
            await SeedUserAsync(userManager, "sandi@sandi.sandi", "sandi@sandi.sandi", "Sandra", "Krusteva", "123456", role: GlobalConstants.GlobalConstants.TrainerRoleName);
        }

        private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, 
            string email = null, string userName = null, string firstName = null, string lastName = null, string password = null,
            string profilePicture = null, string role = null)
        {
            var userExists = await userManager.FindByNameAsync(userName);
            if (userExists == null)
            {
                var user = new ApplicationUser()
                { 
                    Id = Guid.NewGuid().ToString(),
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    ProfilePicture = profilePicture,
                    UserName = userName,
                };

                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
                else
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
