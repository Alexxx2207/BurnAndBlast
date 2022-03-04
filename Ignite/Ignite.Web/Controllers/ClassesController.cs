using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ignite.Web.Controllers
{
    public class ClassesController : Controller
    {
        private readonly ILogger<ClassesController> _logger;

        public ClassesController(ILogger<ClassesController> logger)
        {
            _logger = logger;
        }

        public IActionResult All()
        {
            return View();
        }

        public IActionResult Info(string classId)
        {
            return View();
        }

        // When a button Attend is clicked
        [Authorize]
        public IActionResult Attend(string classId)
        {
            return Redirect("/Classes/All");
        }
    }
}
