using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ignite.Data;
using Ignite.Models.Enums;
using Ignite.Models.ViewModels.Subscriptions;
using Ignite.Services.CartProducts;
using Ignite.Services.Products;
using Ignite.Services.Subscriptions;
using Ignite.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Ignite.UnitTests.Controllers
{
    public class SubscriptionsControllerUnitTests
    {
        [Fact]
        public void AllActionReturnsCheck()
        {
            var cartProductsService = new Mock<ICartProductsService>();
            var productsService = new Mock<IProductsService>();
            var subscriptionsService = new Mock<ISubscriptionsService>();

            var subscriptionController = new SubscriptionsController(subscriptionsService.Object, cartProductsService.Object, productsService.Object);

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

            var productId = Guid.NewGuid().ToString();

            var list = new List<AllSubscriptionsViewModel>();

            list.Add(new AllSubscriptionsViewModel
            { 
                Guid = productId
            });

            subscriptionController.ControllerContext = context;

            productsService.Setup(c => c.GetProductByGUID(productId))
                .Returns(new Models.Product { Price = 10 });

            subscriptionsService.Setup(c => c.GetAllSubscriptions(It.IsAny<string>()))
                .Returns(list);

            var result = subscriptionController.All();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<IEnumerable<AllSubscriptionsViewModel>>(viewResult.ViewData.Model);
        }
        
        [Fact]
        public void BuyActionReturnsCheck()
        {
            var cartProductsService = new Mock<ICartProductsService>();
            var productsService = new Mock<IProductsService>();
            var subscriptionsService = new Mock<ISubscriptionsService>();

            var subscriptionController = new SubscriptionsController(subscriptionsService.Object, cartProductsService.Object, productsService.Object);

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

            subscriptionController.ControllerContext = context;

            cartProductsService.Setup(c => c.AddToCart(It.IsAny<string>(), ProductType.Subscription, It.IsAny<string>()))
                .Callback(() => { });

            var result = subscriptionController.Buy(It.IsAny<string>());

            Assert.IsType<RedirectResult>(result);

            cartProductsService.Setup(c => c.AddToCart(It.IsAny<string>(), ProductType.Subscription, It.IsAny<string>()))
                .Callback(() => throw new ArgumentException());

            result = subscriptionController.Buy(It.IsAny<string>());

            Assert.IsType<RedirectResult>(result);
        }
    }
}
