using Ignite.Services.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ignite.Web.Controllers
{
    [Authorize]
    public class ClassesController : Controller
    {
        private readonly ILogger<ClassesController> _logger;
        private readonly IClassesService classesService;

        public ClassesController(
            ILogger<ClassesController> logger,
            IClassesService classesService)
        {
            _logger = logger;
            this.classesService = classesService;
        }

        public IActionResult All()
        {
            var model = classesService.GetAllClasses(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(model);
        }

        public IActionResult Attend(string classId)
        {
            // Make Buying logic 
            classesService.AddUserToClass(User.FindFirstValue(ClaimTypes.NameIdentifier), classId);

            return Redirect("/Classes/All");
        }

        public IActionResult Details(string classId)
        {
            var model = classesService.GetDetailsOfClass(User.FindFirstValue(ClaimTypes.NameIdentifier), classId);

            return this.View(model);
        }
    }
}
