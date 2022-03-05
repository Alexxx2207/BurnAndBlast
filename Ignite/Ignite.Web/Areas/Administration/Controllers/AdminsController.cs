using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ignite.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Area("Administration")]
    public class AdminsController : Controller
    {
        private readonly ILogger<AdminsController> _logger;

        public AdminsController(ILogger<AdminsController> logger)
        {
            _logger = logger;
        }

        public IActionResult AddFitness()
        {
            return View();
        }

        public IActionResult AddEvent()
        {
            return View();
        }

        public IActionResult AddClass()
        {
            return View();
        }
    }
}
