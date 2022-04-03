using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ignite.Models.ParentModels;
using Ignite.Models.ViewModels.Events;
using Ignite.Services.Events;
using Ignite.Web.Areas.Administration.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Ignite.UnitTests.Controllers
{
    public class AdminEventsUnitTests
    {
        [Fact]
        public void AllEventsReturnLocalRedirect()
        {
            var eventsService = new Mock<IEventsService>();

            var adminClassesController = new AdminEventsController(eventsService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            adminClassesController.ControllerContext = context;

            eventsService.Setup(c => c.GetEvents(It.IsAny<string>()))
                .Returns(new List<ShowEventsViewModel>());

            var result = adminClassesController.AllEvents();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<AllEventsParentModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void AddEventReturnLocalRedirect()
        {
            var eventsService = new Mock<IEventsService>();

            var adminClassesController = new AdminEventsController(eventsService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            adminClassesController.ControllerContext = context;

            AllEventsParentModel model = new AllEventsParentModel()
            {
                AddEventInputModel = new Models.InputModels.Events.AddEventInputModel()
                {
                    Name = "name",
                    StartingDateTime = DateTime.Now,
                }
            };

            eventsService.Setup(c => c.IsNameAvailable(model.AddEventInputModel.Name, null))
                .Returns(true);

            var result = adminClassesController.AddEvents(model);

            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public void AddEventThrowsErrorName()
        {
            var eventsService = new Mock<IEventsService>();

            var adminClassesController = new AdminEventsController(eventsService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            adminClassesController.ControllerContext = context;

            AllEventsParentModel model = new AllEventsParentModel()
            {
                AddEventInputModel = new Models.InputModels.Events.AddEventInputModel()
                {
                    Name = "name",
                    StartingDateTime = DateTime.Now,
                }
            };

            eventsService.Setup(c => c.IsNameAvailable(model.AddEventInputModel.Name, null))
                .Returns(false);

            var result = adminClassesController.AddEvents(model);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void RemoveEventReturnRedirectsCheck()
        {
            var eventsService = new Mock<IEventsService>();

            var adminClassesController = new AdminEventsController(eventsService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            adminClassesController.ControllerContext = context;

            string classId = Guid.NewGuid().ToString();

            eventsService.Setup(c => c.CheckEventExists(classId))
                .Returns(true);

            var result = adminClassesController.RemoveEvent(classId);

            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public void ChangeEventReturnViewCheck()
        {
            var eventsService = new Mock<IEventsService>();

            var adminClassesController = new AdminEventsController(eventsService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            adminClassesController.ControllerContext = context;

            string classId = Guid.NewGuid().ToString();

            eventsService.Setup(c => c.GetEventByGUID(classId))
                .Returns(new Models.Event());

            var result = adminClassesController.ChangeEvent(classId);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ChangeEventsParentModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void ChangeEventReturnRedirectCheck()
        {
            var eventsService = new Mock<IEventsService>();

            var adminClassesController = new AdminEventsController(eventsService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            adminClassesController.ControllerContext = context;

            string classId = Guid.NewGuid().ToString();

            eventsService.Setup(c => c.GetEventByGUID(classId))
                .Returns(new Models.Event());

            eventsService.Setup(c => c.IsNameAvailable("name", classId))
                .Returns(true);

            ChangeEventsParentModel model = new ChangeEventsParentModel
            {
                InputModel = new Models.InputModels.Events.ChangeEventInputModel
                {
                    Guid = classId,
                    Name = "name"
                }
            };

            var result = adminClassesController.ChangeEvent(model);

            Assert.IsType<RedirectResult>(result);

            eventsService.Setup(c => c.IsNameAvailable("name", classId))
                .Returns(false);

            result = adminClassesController.ChangeEvent(model);

            Assert.IsType<ViewResult>(result);
        }
    }
}
