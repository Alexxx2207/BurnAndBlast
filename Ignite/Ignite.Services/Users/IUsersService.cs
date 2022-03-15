using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Services.Users
{
    public interface IUsersService
    {
        Task SaveImage(IFormFile file);

        void RemoveImage(string userId);
    }
}
