using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Ignite.Web.Controllers;
using Ignite.Services.Events;
using Ignite.Services.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Ignite.Models.ParentModels;
using Microsoft.EntityFrameworkCore;
using Ignite.Data;
using Ignite.Web.Models;

namespace Ignite.UnitTests.Controllers
{
    public class HomeControllerUnitTests
    {
        [Fact]
        public void IndexPageReturnView()
        {
            var eventsService = new Mock<IEventsService>();
            var classesService = new Mock<IClassesService>();

            var homeController = new HomeController(eventsService.Object, classesService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());
            
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            homeController.ControllerContext = context;

            var result = homeController.Index();
            Assert.IsType<ViewResult>(result);

        }
        
        [Fact]
        public void IndexPageRedirectBecauseUserIsLogged()
        {
            var eventsService = new Mock<IEventsService>();
            var classesService = new Mock<IClassesService>();

            var homeController = new HomeController(eventsService.Object, classesService.Object);

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

            homeController.ControllerContext = context;

            var result = homeController.Index();
            Assert.IsType<RedirectResult>(result);

        }

        [Fact]
        public void LoggedInIndexPageReturnView()
        {
            var eventsService = new Mock<IEventsService>();
            var classesService = new Mock<IClassesService>();

            var homeController = new HomeController(eventsService.Object, classesService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            homeController.ControllerContext = context;

            var result = homeController.IndexLoggedIn();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<LoggedInHomePageParentModel>(viewResult.ViewData.Model);

            Assert.IsType<LoggedInHomePageParentModel>(model);
        }
        
        [Fact]
        public void ErrorReturnsViewWithErrorViewModel()
        {
            var eventsService = new Mock<IEventsService>();
            var classesService = new Mock<IClassesService>();

            var homeController = new HomeController(eventsService.Object, classesService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            homeController.ControllerContext = context;

            var result = homeController.Error();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ErrorViewModel>(viewResult.ViewData.Model);

            Assert.IsType<ErrorViewModel>(model);
        }
    }
}
