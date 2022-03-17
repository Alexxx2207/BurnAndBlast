using Ignite.Models.ParentModels;
using Ignite.Services.Fitnesses;
using Ignite.Models.InputModels.Fitnesses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ignite.Models.InputModels.Events;
using Ignite.Services.Events;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ignite.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Area("Administration")]
    public class AdminsController : Controller
    {
        private readonly ILogger<AdminsController> _logger;
        private readonly IFitnessService fitnessService;
        private readonly IEventsService eventsService;

        public AdminsController(
            ILogger<AdminsController> logger,
            IFitnessService fitnessService,
            IEventsService eventsService)
        {
            _logger = logger;
            this.fitnessService = fitnessService;
            this.eventsService = eventsService;
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
            if(fitnessService.CheckFitnessExist(fitnessId))
                fitnessService.RemoveFitness(fitnessId);

            return Redirect("/Administration/Admins/AllFitnesses");
        }

        public IActionResult AllEvents()
        {

            var model = new AllEventsParentModel
            {
                AddEventInputModel = new AddEventInputModel(),
                ShowEventsViewModel = eventsService.GetEvents(User.FindFirstValue(ClaimTypes.NameIdentifier))
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult AddEvents(AllEventsParentModel model)
        {
            try
            {
                eventsService.AddEvent(model.AddEventInputModel);
            }
            catch (Exception e)
            {

                var parentModel = new AllEventsParentModel()
                {
                    AddEventInputModel = new AddEventInputModel(),
                    ShowEventsViewModel = eventsService.GetEvents(User.FindFirstValue(ClaimTypes.NameIdentifier))
                };

                return View("AllEvents", parentModel);
            }
            return Redirect("/Events/All");
        }

        public IActionResult AllClasses()
        {
            return View();
        }
    }
}
