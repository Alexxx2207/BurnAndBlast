using Ignite.Services.Fitnesses;
using Microsoft.AspNetCore.Mvc;

namespace Ignite.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class FitnessesController : Controller
    {
        private readonly IFitnessService fitnessService;

        public FitnessesController(
            IFitnessService fitnessService)
        {
            this.fitnessService = fitnessService;
        }

        public IActionResult All()
        {
            var viewModel = fitnessService.GetAllFitnesses();

            return View(viewModel);
        }
    }
}
