using Ignite.Models.ViewModels.Subscriptions;
using Ignite.Services.CartProducts;
using Ignite.Services.Products;
using Ignite.Services.Subscriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ignite.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class SubscriptionsController : Controller
    {
        private readonly ISubscriptionsService subscriptionService;
        private readonly ICartProductsService cartProductsService;
        private readonly IProductsService productsService;

        public SubscriptionsController(
            ISubscriptionsService subscriptionService,
            ICartProductsService cartProductsService,
            IProductsService productsService)
        {
            this.subscriptionService = subscriptionService;
            this.cartProductsService = cartProductsService;
            this.productsService = productsService;
        }

        public IActionResult All()
        {
            var model = subscriptionService
                            .GetAllSubscriptions(User.FindFirstValue(ClaimTypes.NameIdentifier))
                            .OrderBy(s => s.OrderInPage)
                            .ToList();

            foreach (var sub in model)
            {
                sub.Price = productsService.GetProductByGUID(sub.Guid).Price;
            }


            return View(model);
        }

        //When clicking a button Purchase
        [Authorize]
        public IActionResult Buy(string subId)
        {
            try
            {
                cartProductsService.AddToCart(User.FindFirstValue(ClaimTypes.NameIdentifier),
                                       Ignite.Models.Enums.ProductType.Subscription,
                                       subId);
            }
            catch (Exception)
            {
                return Redirect("/Products/CheckOut");
            }

            return Redirect("/Products/CheckOut");
        }
    }
}
