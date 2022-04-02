using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ignite.Models.Enums;
using Ignite.Models.ViewModels.Classes;
using Ignite.Services.CartProducts;
using Ignite.Services.Classes;
using Ignite.Services.Products;
using Ignite.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Ignite.UnitTests.Controllers
{
    public class ClassControllerUnitTests
    {
        [Fact]
        public void AllActionReturnsCheck()
        {
            var productsService = new Mock<ICartProductsService>();
            var classesService = new Mock<IClassesService>();

            var classController = new ClassesController(classesService.Object, productsService.Object);

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

            classController.ControllerContext = context;

            classesService.Setup(c => c.GetAllClasses(It.IsAny<string>()))
                .Returns(new List<AllClassesViewModel>());

            var result = classController.All();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<IEnumerable<AllClassesViewModel>>(viewResult.ViewData.Model);
        }
        
        [Fact]
        public void AttendActionReturnsCheck()
        {
            var productsService = new Mock<ICartProductsService>();
            var classesService = new Mock<IClassesService>();

            var classController = new ClassesController(classesService.Object, productsService.Object);

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

            classController.ControllerContext = context;

            productsService.Setup(c => c.AddToCart(It.IsAny<string>(), ProductType.Class, It.IsAny<string>()))
                .Callback(() => { });

            var result = classController.Attend(Guid.NewGuid().ToString());

            Assert.IsType<RedirectResult>(result);
        }
        
        [Fact]
        public void AttendActionThrowsError()
        {
            var productsService = new Mock<ICartProductsService>();
            var classesService = new Mock<IClassesService>();

            var classController = new ClassesController(classesService.Object, productsService.Object);

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

            classController.ControllerContext = context;

            productsService.Setup(c => c.AddToCart(It.IsAny<string>(), ProductType.Class, It.IsAny<string>()))
                .Callback(() => throw new InvalidCastException());

            Assert.IsType<RedirectResult>(classController.Attend(It.IsAny<string>()));

            productsService.Setup(c => c.AddToCart(It.IsAny<string>(), ProductType.Class, It.IsAny<string>()))
                .Callback(() => throw new ArgumentException());

            Assert.IsType<RedirectResult>(classController.Attend(It.IsAny<string>()));
        }
        
        [Fact]
        public void DetailsActionReturnsCheck()
        {
            var productsService = new Mock<ICartProductsService>();
            var classesService = new Mock<IClassesService>();

            var classController = new ClassesController(classesService.Object, productsService.Object);

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

            classController.ControllerContext = context;

            classesService.Setup(c => c.GetDetailsOfClass(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ShowClassDetailsViewModel());

            var result = classController.Details(Guid.NewGuid().ToString());

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ShowClassDetailsViewModel>(viewResult.ViewData.Model);
        }
    }
}
