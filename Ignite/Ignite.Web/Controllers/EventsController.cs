using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ignite.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly ILogger<EventsController> _logger;

        public EventsController(ILogger<EventsController> logger)
        {
            _logger = logger;
        }

        public IActionResult All()
        {
            return View();
        }

        public IActionResult Info(string eventId)
        {
            return View();
        }

        // When a button Attend is clicked
        [Authorize]
        public IActionResult Attend(string classId)
        {
            return Redirect("/Events/All");
        }


    }
}
