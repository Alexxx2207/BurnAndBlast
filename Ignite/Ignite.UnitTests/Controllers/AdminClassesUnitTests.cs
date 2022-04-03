using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ignite.Web.Areas.Administration.Controllers;
using Ignite.Services.Classes;
using Ignite.Services.Products;
using Moq;
using Xunit;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Ignite.Models.ParentModels;
using Ignite.Models.ViewModels.Classes;

namespace Ignite.UnitTests.Controllers
{
    public class AdminClassesUnitTests
    {
        [Fact]
        public void AllClassesReturnLocalRedirect()
        {
            var productsService = new Mock<IProductsService>();
            var classesService = new Mock<IClassesService>();

            var adminClassesController = new AdminClassesController(classesService.Object, productsService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            adminClassesController.ControllerContext = context;

            classesService.Setup(c => c.GetAllClasses(It.IsAny<string>()))
                .Returns(new List<AllClassesViewModel>());

            var result = adminClassesController.AllClasses();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<AllClassesParentModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void AddClassReturnLocalRedirect()
        {
            var productsService = new Mock<IProductsService>();
            var classesService = new Mock<IClassesService>();

            var adminClassesController = new AdminClassesController(classesService.Object, productsService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            adminClassesController.ControllerContext = context;

            AllClassesParentModel model = new AllClassesParentModel()
            {
                AddClassInputModel = new Models.InputModels.Classes.AddClassInputModel()
                { 
                    Name = "name",
                    StartingDateTime = DateTime.Now,
                }
            };

            classesService.Setup(c => c.IsNameAvailable(model.AddClassInputModel.Name, null))
                .Returns(true);

            var result = adminClassesController.AddClass(model);

            Assert.IsType<RedirectResult>(result);
        }
        
        [Fact]
        public void AddClassThrowsErrorName()
        {
            var productsService = new Mock<IProductsService>();
            var classesService = new Mock<IClassesService>();

            var adminClassesController = new AdminClassesController(classesService.Object, productsService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            adminClassesController.ControllerContext = context;

            AllClassesParentModel model = new AllClassesParentModel()
            {
                AddClassInputModel = new Models.InputModels.Classes.AddClassInputModel()
                { 
                    Name = "name",
                    StartingDateTime = DateTime.Now,
                }
            };

            classesService.Setup(c => c.IsNameAvailable(model.AddClassInputModel.Name, null))
                .Returns(false);

            var result = adminClassesController.AddClass(model);

            Assert.IsType<ViewResult>(result);
        }
        
        [Fact]
        public void AddClassThrowsErrorStartDate()
        {
            var productsService = new Mock<IProductsService>();
            var classesService = new Mock<IClassesService>();

            var adminClassesController = new AdminClassesController(classesService.Object, productsService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            adminClassesController.ControllerContext = context;

            AllClassesParentModel model = new AllClassesParentModel()
            {
                AddClassInputModel = new Models.InputModels.Classes.AddClassInputModel()
                { 
                    Name = "name",
                }
            };

            classesService.Setup(c => c.IsNameAvailable(model.AddClassInputModel.Name, null))
                .Returns(true);

            var result = adminClassesController.AddClass(model);

            Assert.IsType<ViewResult>(result);
        }
        
        [Fact]
        public void RemoveClassReturnRedirectsCheck()
        {
            var productsService = new Mock<IProductsService>();
            var classesService = new Mock<IClassesService>();

            var adminClassesController = new AdminClassesController(classesService.Object, productsService.Object);

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

            classesService.Setup(c => c.CheckClassExists(classId))
                .Returns(true);

            var result = adminClassesController.RemoveClass(classId);

            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public void ChangeClassReturnViewCheck()
        {
            var productsService = new Mock<IProductsService>();
            var classesService = new Mock<IClassesService>();

            var adminClassesController = new AdminClassesController(classesService.Object, productsService.Object);

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

            classesService.Setup(c => c.GetClassByGUID(classId))
                .Returns(new Models.Class());

            productsService.Setup(c => c.GetProductByGUID(classId))
                .Returns(new Models.Product(){Price = 10});

            var result = adminClassesController.ChangeClass(classId);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ChangeClassesParentModel>(viewResult.ViewData.Model);
        }
        
        [Fact]
        public void ChangeClassReturnRedirectCheck()
        {
            var productsService = new Mock<IProductsService>();
            var classesService = new Mock<IClassesService>();

            var adminClassesController = new AdminClassesController(classesService.Object, productsService.Object);

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

            classesService.Setup(c => c.GetClassByGUID(classId))
                .Returns(new Models.Class());

            classesService.Setup(c => c.IsNameAvailable("name", classId))
                .Returns(true);

            ChangeClassesParentModel model = new ChangeClassesParentModel
            { 
                InputModel = new Models.InputModels.Classes.ChangeClassInputModel
                {
                    Guid = classId,
                    Name = "name"
                }
            };

            var result = adminClassesController.ChangeClass(model);

            Assert.IsType<RedirectResult>(result);

            classesService.Setup(c => c.IsNameAvailable("name", classId))
                .Returns(false);

            result = adminClassesController.ChangeClass(model);

            Assert.IsType<ViewResult>(result);
        }
    }
}
