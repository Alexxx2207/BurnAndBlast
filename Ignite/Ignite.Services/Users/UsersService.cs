using Ignite.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext db;

        public UsersService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void RemoveImage(string userId)
        {
            var user = db.Users.Find(userId);

            File.Delete($"wwwroot/Profile Pics/{user.ProfilePicture}");

            user.ProfilePicture = null;
            db.SaveChanges();
        }

        public async Task SaveImage(IFormFile file)
        {
            if (file.Length > 0)
            {
                using (var stream = new FileStream("~/Profile Pics", FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
        }
    }
}
