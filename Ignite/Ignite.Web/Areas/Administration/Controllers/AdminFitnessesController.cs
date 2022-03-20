using Ignite.Models.ParentModels;
using Ignite.Services.Fitnesses;
using Ignite.Models.InputModels.Fitnesses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ignite.Models.InputModels.Events;
using Ignite.Services.Events;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Ignite.Models.ViewModels.Events;

namespace Ignite.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Area("Administration")]
    public class AdminFitnessesController : Controller
    {
        private readonly ILogger<AdminFitnessesController> _logger;
        private readonly IFitnessService fitnessService;

        public AdminFitnessesController(
            ILogger<AdminFitnessesController> logger,
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
            try
            {
                fitnessService.AddFitness(model.FitnessesInputModel);
            }
            catch (Exception e)
            {
                var parentModel = new AllFitnessParentModel()
                {
                    FitnessesInputModel = new FitnessesInputModel(),
                    GetFitnessViewModels = fitnessService.GetAllFitnesses()
                };

                return View("AllFitnesses", parentModel);
            }
            return Redirect("/Fitnesses/All");
        }

        public IActionResult RemoveFitness(string fitnessId)
        {
            if (fitnessService.CheckFitnessExist(fitnessId))
                fitnessService.RemoveFitness(fitnessId);

            return Redirect("/Administration/AdminFitnesses/AllFitnesses");
        }

        
    }
}
