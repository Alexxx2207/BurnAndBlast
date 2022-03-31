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
        private readonly IEventsService eventsService;

        public AdminEventsController(
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
            ModelState.Remove("ShowEventsViewModel");

            if (!string.IsNullOrWhiteSpace(model.AddEventInputModel.Name) &&
                !eventsService.IsNameAvailable(model.AddEventInputModel.Name, null))
            {
                ModelState.AddModelError("nameExists", $"Event with name '{model.AddEventInputModel.Name}' already exists!");
            }
            if (model.AddEventInputModel.StartingDateTime == null)
            {
                ModelState.AddModelError("dateMissing", $"The Start Date & Time field is required.");
                ModelState.Remove("AddEventInputModel.StartingDateTime");
            }

            if (!ModelState.IsValid)
            {
                var parentModel = new AllEventsParentModel()
                {
                    AddEventInputModel = model.AddEventInputModel,
                    ShowEventsViewModel = eventsService.GetEvents(User.FindFirstValue(ClaimTypes.NameIdentifier))
                };

                return View("AllEvents", parentModel);

            }

            eventsService.AddEvent(model.AddEventInputModel);

            return Redirect("/Events/All");
        }

        public IActionResult RemoveEvent(string eventId)
        {
            if (eventsService.CheckEventExists(eventId))
                eventsService.RemoveEvent(User.FindFirstValue(ClaimTypes.NameIdentifier), eventId);

            return Redirect("/Administration/AdminEvents/AllEvents");
        }

        public IActionResult ChangeEvent(string eventId)
        {
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
            ModelState.Remove("ViewModel");

            if (!string.IsNullOrWhiteSpace(model.InputModel.Name) &&
                !eventsService.IsNameAvailable(model.InputModel.Name, model.InputModel.Guid))
            {
                ModelState.AddModelError("nameExists", $"Event with name '{model.InputModel.Name}' already exists!");
            }
            if (model.InputModel.StartingDateTime == null)
            {
                ModelState.AddModelError("dateMissing", $"A starting Date & Time is required.");
            }

            if(!ModelState.IsValid)
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

                return View("ChangeEvent", model);
            }

            eventsService.ChangeEvent(model.InputModel);


            return Redirect("/Administration/AdminEvents/AllEvents");
        }

    }
}
