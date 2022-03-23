using Ignite.Models.InputModels.Classes;
using Ignite.Models.ParentModels;
using Ignite.Services.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ignite.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Area("Administration")]
    public class AdminClassesController : Controller
    {
        private readonly ILogger<AdminFitnessesController> _logger;
        private readonly IClassesService classesService;

        public AdminClassesController(
            ILogger<AdminFitnessesController> logger,
            IClassesService classesService)
        {
            _logger = logger;
            this.classesService = classesService;
        }

        public IActionResult AllClasses()
        {
            var model = new AllClassesParentModel
            {
                AddClassInputModel = new AddClassInputModel(),
                ShowClassesViewModel = classesService.GetAllClasses(User.FindFirstValue(ClaimTypes.NameIdentifier))
            };

            return View(model);
        }
    }
}
