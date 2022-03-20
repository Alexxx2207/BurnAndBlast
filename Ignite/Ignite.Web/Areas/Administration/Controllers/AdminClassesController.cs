using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ignite.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Area("Administration")]
    public class AdminClassesController : Controller
    {
        private readonly ILogger<AdminFitnessesController> _logger;

        public AdminClassesController(
            ILogger<AdminFitnessesController> logger)
        {
            _logger = logger;
        }

        public IActionResult AllClasses()
        {
            return View();
        }
    }
}
