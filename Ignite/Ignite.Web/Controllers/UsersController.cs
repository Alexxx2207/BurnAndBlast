using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ignite.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return this.Redirect("/Home/IndexLoggedIn");

            return this.LocalRedirect("/Identity/Account/Login");
        }
        
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return this.Redirect("/Home/IndexLoggedIn");

            return this.LocalRedirect("/Identity/Account/Register");

        }
    }
}
