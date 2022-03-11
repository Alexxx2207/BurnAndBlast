using Ignite.Services.Fitnesses;
using Microsoft.AspNetCore.Mvc;

namespace Ignite.Web.Controllers
{
    public class FitnessesController : Controller
    {
        private readonly ILogger<FitnessesController> _logger;
        private readonly IFitnessService fitnessService;

        public FitnessesController(
            ILogger<FitnessesController> logger,
            IFitnessService fitnessService)
        {
            _logger = logger;
            this.fitnessService = fitnessService;
        }

        public IActionResult All()
        {
            var viewModel = fitnessService.GetAllFitnesses();

            return View(viewModel);
        }
    }
}
