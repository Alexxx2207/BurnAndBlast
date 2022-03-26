using Ignite.Models.ParentModels;
using Ignite.Services.Classes;
using Ignite.Services.Events;
using Ignite.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Ignite.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEventsService eventsService;
        private readonly IClassesService classesService;

        public HomeController(
            ILogger<HomeController> logger,
            IEventsService eventsService,
            IClassesService classesService)
        {
            _logger = logger;
            this.eventsService = eventsService;
            this.classesService = classesService;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return this.Redirect("/Home/IndexLoggedIn");
            }

            return View();
        }

        [Authorize]
        public IActionResult IndexLoggedIn()
        {
            var model = new LoggedInHomePageParentModel
            {
                TopClasses = classesService
                                .GetTopClasses(User.FindFirstValue(ClaimTypes.NameIdentifier),
                                                GlobalConstants.GlobalConstants.TopClassesCount),
                TopEvents = eventsService
                                .GetTopEvents(User.FindFirstValue(ClaimTypes.NameIdentifier),
                                                GlobalConstants.GlobalConstants.TopEventsCount),
            };

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}