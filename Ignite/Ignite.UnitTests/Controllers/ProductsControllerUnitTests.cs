using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ignite.Models.ViewModels.Products;
using Ignite.Services.CartProducts;
using Ignite.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Ignite.UnitTests.Controllers
{
    public class ProductsControllerUnitTests
    {
        [Fact]
        public void CheckOutActionReturnsCheck()
        {
            var productsService = new Mock<ICartProductsService>();

            var productsController = new ProductsController(productsService.Object);

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

            productsController.ControllerContext = context;

            productsService.Setup(c => c.GetAllProductsForTheUser(It.IsAny<string>()))
                .Returns(new List<ProductInCartViewModel>());

            var result = productsController.CheckOut();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<IEnumerable<ProductInCartViewModel>>(viewResult.ViewData.Model);
        }
        
        [Fact]
        public void RemoveActionReturnsCheck()
        {
            var productsService = new Mock<ICartProductsService>();

            var productsController = new ProductsController(productsService.Object);

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

            productsController.ControllerContext = context;

            var result = productsController.Remove(It.IsAny<string>());
            Assert.IsType<RedirectResult>(result);
        }
        
        [Fact]
        public void PurchaseActionReturnsCheck()
        {
            var productsService = new Mock<ICartProductsService>();

            var productsController = new ProductsController(productsService.Object);

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

            productsController.ControllerContext = context;

            var result = productsController.Purchase();
            Assert.IsType<RedirectResult>(result);
        }
    }
}
