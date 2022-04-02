using Ignite.Models;
using Ignite.Services.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ignite.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class EventsController : Controller
    {
        private readonly IEventsService eventsService;

        public EventsController(
            IEventsService eventsService)
        {
            this.eventsService = eventsService;
        }

        public IActionResult All()
        {
            var model = eventsService.GetEvents(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(model);
        }

        // When a button Subscribe is clicked
        [Authorize]
        public IActionResult Attend(string eventId)
        {
            try
            {
                eventsService.AddUserToEvent(User.FindFirstValue(ClaimTypes.NameIdentifier), eventId);
            }
            catch (Exception)
            {
                return Redirect("/Events/All");
            }

            return Redirect("/Events/All");
        }

        // When a button Unsubscribe is clicked

        [Authorize]
        public IActionResult UnAttend(string eventId)
        {
            try
            {
                eventsService.RemoveUserFromEvent(User.FindFirstValue(ClaimTypes.NameIdentifier), eventId);
            }
            catch (Exception)
            {
                return Redirect("/Events/All");
            }

            return Redirect("/Events/All");
        }

        // When a button More Details is clicked
        public IActionResult Details(string eventId)
        {
            var model = eventsService.GetDetailsOfEvent(User.FindFirstValue(ClaimTypes.NameIdentifier), eventId);
            
            return this.View(model);
        }
    }
}
