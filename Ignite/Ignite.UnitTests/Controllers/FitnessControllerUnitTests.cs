using Ignite.Models.ViewModels.Fitnesses;
using Ignite.Services.Fitnesses;
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
    public class FitnessControllerUnitTests
    {
        [Fact]
        public void AllActionReturnsCheck()
        {
            var eventsService = new Mock<IFitnessService>();

            var eventsController = new FitnessesController(eventsService.Object);

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

            eventsService.Setup(c => c.GetAllFitnesses())
                .Returns(new List<GetFitnessViewModel>());

            var result = eventsController.All();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<IEnumerable<GetFitnessViewModel>>(viewResult.ViewData.Model);
        }
    }
}
