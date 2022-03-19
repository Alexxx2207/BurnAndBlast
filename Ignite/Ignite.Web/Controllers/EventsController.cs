﻿using Ignite.Models;
using Ignite.Services.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ignite.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IEventsService eventsService;

        public EventsController(
            ILogger<EventsController> logger,
            IEventsService eventsService)
        {
            _logger = logger;
            this.eventsService = eventsService;
        }

        public IActionResult All()
        {
            var model = eventsService.GetEvents(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(model);
        }

        // When a button Attend is clicked
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
    }
}
