using Ignite.Models.ViewModels.Events;
using Ignite.Services.Events;
using Ignite.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ignite.UnitTests.Controllers
{
    public class EventControllerUnitTests
    {
        [Fact]
        public void AllActionReturnsCheck()
        {
            var eventsService = new Mock<IEventsService>();

            var eventsController = new EventsController(eventsService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "identifier"),
                    new Claim(ClaimTypes.Name, "qa@qa.qa"),
                }, "TestAuthentication"));

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            eventsController.ControllerContext = context;

            eventsService.Setup(c => c.GetEvents(It.IsAny<string>()))
                .Returns(new List<ShowEventsViewModel>());

            var result = eventsController.All();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<IEnumerable<ShowEventsViewModel>>(viewResult.ViewData.Model);
        }

        [Fact]
        public void AttendActionReturnsCheck()
        {
            var eventsService = new Mock<IEventsService>();

            var eventsController = new EventsController(eventsService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "identifier"),
                    new Claim(ClaimTypes.Name, "qa@qa.qa"),
                }, "TestAuthentication"));

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            eventsController.ControllerContext = context;

            eventsService.Setup(c => c.AddUserToEvent(It.IsAny<string>(), It.IsAny<string>()))
                .Callback(() => { });

            var result = eventsController.Attend(Guid.NewGuid().ToString());

            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public void AttendActionThrowsError()
        {
            var eventsService = new Mock<IEventsService>();

            var eventsController = new EventsController(eventsService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "identifier"),
                    new Claim(ClaimTypes.Name, "qa@qa.qa"),
                }, "TestAuthentication"));

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            eventsController.ControllerContext = context;

            eventsService.Setup(c => c.AddUserToEvent(It.IsAny<string>(), It.IsAny<string>()))
                .Callback(() => throw new ArgumentException());

            Assert.IsType<RedirectResult>(eventsController.Attend(It.IsAny<string>()));
        }
        
        [Fact]
        public void UnattendActionReturnsCheck()
        {
            var eventsService = new Mock<IEventsService>();

            var eventsController = new EventsController(eventsService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "identifier"),
                    new Claim(ClaimTypes.Name, "qa@qa.qa"),
                }, "TestAuthentication"));

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            eventsController.ControllerContext = context;

            eventsService.Setup(c => c.AddUserToEvent(It.IsAny<string>(), It.IsAny<string>()))
                .Callback(() => { });

            var result = eventsController.UnAttend(Guid.NewGuid().ToString());

            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public void UnattendActionThrowsError()
        {
            var eventsService = new Mock<IEventsService>();

            var eventsController = new EventsController(eventsService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "identifier"),
                    new Claim(ClaimTypes.Name, "qa@qa.qa"),
                }, "TestAuthentication"));

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            eventsController.ControllerContext = context;

            eventsService.Setup(c => c.RemoveUserFromEvent(It.IsAny<string>(), It.IsAny<string>()))
                .Callback(() => throw new ArgumentException());

            Assert.IsType<RedirectResult>(eventsController.UnAttend(It.IsAny<string>()));
        }

        [Fact]
        public void DetailsActionReturnsCheck()
        {
            var eventsService = new Mock<IEventsService>();

            var eventsController = new EventsController(eventsService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "identifier"),
                    new Claim(ClaimTypes.Name, "qa@qa.qa"),
                }, "TestAuthentication"));

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            eventsController.ControllerContext = context;

            eventsService.Setup(c => c.GetDetailsOfEvent(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ShowEventDetailsViewModel());

            var result = eventsController.Details(Guid.NewGuid().ToString());

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ShowEventDetailsViewModel>(viewResult.ViewData.Model);
        }
    }
}
