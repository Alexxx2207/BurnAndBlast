using Microsoft.AspNetCore.Mvc;

namespace Ignite.Web.Controllers
{
    public class FitnessesController : Controller
    {
        private readonly ILogger<FitnessesController> _logger;

        public FitnessesController(ILogger<FitnessesController> logger)
        {
            _logger = logger;
        }

        public IActionResult All()
        {
            return View();
        }
    }
}
