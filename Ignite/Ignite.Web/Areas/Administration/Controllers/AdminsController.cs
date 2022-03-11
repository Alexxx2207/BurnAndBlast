using Ignite.Models.ParentModels;
using Ignite.Services.Fitnesses;
using Ignite.Models.InputModels.Fitnesses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ignite.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Area("Administration")]
    public class AdminsController : Controller
    {
        private readonly ILogger<AdminsController> _logger;
        private readonly IFitnessService fitnessService;

        public AdminsController(
            ILogger<AdminsController> logger,
            IFitnessService fitnessService)
        {
            _logger = logger;
            this.fitnessService = fitnessService;
        }

        public IActionResult AllFitnesses()
        {
            var viewModel = fitnessService.GetAllFitnesses();

            var parentModel = new AllFitnessParentModel()
            {
                FitnessesInputModel = new FitnessesInputModel(),
                GetFitnessViewModels = viewModel
            };

            return View(parentModel);
        }

        [HttpPost]
        public IActionResult AddFitness(AllFitnessParentModel model)
        {
            fitnessService.AddFitness(model.FitnessesInputModel);
            return Redirect("/Fitnesses/All");
        }

        public IActionResult RemoveFitness(string fitnessId)
        { 
            fitnessService.RemoveFitness(fitnessId);

            return Redirect("/Administration/Admins/AllFitnesses");
        }

        public IActionResult AllEvents()
        {
            return View();
        }

        public IActionResult AllClasses()
        {
            return View();
        }
    }
}
