using Ignite.Models.InputModels.Events;
using Ignite.Models.ParentModels;
using Ignite.Models.ViewModels.Events;
using Ignite.Services.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ignite.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Area("Administration")]
    public class AdminEventsController : Controller
    {
        private readonly ILogger<AdminEventsController> _logger;
        private readonly IEventsService eventsService;

        public AdminEventsController(
            ILogger<AdminEventsController> logger,
            IEventsService eventsService)
        {
            this.eventsService = eventsService;

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
            var parentModel = new AllEventsParentModel()
            {
                AddEventInputModel = new AddEventInputModel(),
                ShowEventsViewModel = eventsService.GetEvents(User.FindFirstValue(ClaimTypes.NameIdentifier))
            };

            try
            {
                eventsService.AddEvent(model.AddEventInputModel);
            }
            catch (Exception e)
            {

                return View("AllEvents", parentModel);
            }
            return Redirect("/Events/All");
        }

        public IActionResult RemoveEvent(string eventId)
        {
            if (eventsService.CheckEventExists(eventId))
                eventsService.RemoveEvent(eventId);

            return Redirect("/Administration/AdminEvents/AllEvents");
        }

        public IActionResult ChangeEvent(string eventId)
        {
            if (!eventsService.CheckEventExists(eventId))
                return Redirect("/Administration/AdminEvents/AllEvents");

            var ev = eventsService.GetEventByGUID(eventId);

            var model = new ChangeEventsParentModel
            {
                InputModel = new ChangeEventInputModel(),
                ViewModel = new ChangeEventViewModel
                {
                    Guid = eventId,
                    Address = ev.Address,
                    Name = ev.Name,
                    StartingDateTime = ev.StartingDateTime,
                    Description = ev?.Description,
                }
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult ChangeEvent(ChangeEventsParentModel model)
        {

            var ev = eventsService.GetEventByGUID(model.InputModel.Guid);

            model.ViewModel = new ChangeEventViewModel
            {
                Guid = model.InputModel.Guid,
                Address = ev.Address,
                Name = ev.Name,
                StartingDateTime = ev.StartingDateTime,
                Description = ev?.Description,
            };

            try
            {
                eventsService.ChangeEvent(model.InputModel);
            }
            catch (Exception e)
            {

                return View("ChangeEvent", model);
            }

            return Redirect("/Administration/AdminEvents/AllEvents");
        }

    }
}
