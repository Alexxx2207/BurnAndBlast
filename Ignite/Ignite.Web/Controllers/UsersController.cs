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

        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }
    }
}
