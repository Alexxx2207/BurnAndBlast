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
    [AutoValidateAntiforgeryToken]
    [Authorize(Roles = "Administrator")]
    [Area("Administration")]
    public class AdminFitnessesController : Controller
    {
        private readonly IFitnessService fitnessService;

        public AdminFitnessesController(
            IFitnessService fitnessService)
        {
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
        [AutoValidateAntiforgeryToken]
        public IActionResult AddFitness(AllFitnessParentModel model)
        {
            if(!string.IsNullOrWhiteSpace(model.FitnessesInputModel.Name) && !fitnessService.IsNameAvailable(model.FitnessesInputModel.Name))
            {
                ModelState.AddModelError("nameExists", $"Fitness with name '{model.FitnessesInputModel.Name}' already exists!");
            }

            ModelState.Remove("GetFitnessViewModels");

            if (!ModelState.IsValid)
            {
                var parentModel = new AllFitnessParentModel()
                {
                    FitnessesInputModel = model.FitnessesInputModel,
                    GetFitnessViewModels = fitnessService.GetAllFitnesses()
                };

                return View("AllFitnesses", parentModel);
            }

            fitnessService.AddFitness(model.FitnessesInputModel);
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
