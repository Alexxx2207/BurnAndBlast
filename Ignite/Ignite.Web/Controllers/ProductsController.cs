using Ignite.Services.CartProducts;
using Ignite.Services.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ignite.Web.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class ProductsController : Controller
    {
        private readonly ICartProductsService cartProductsService;

        public ProductsController(
            ICartProductsService cartProductsService)
        {
            this.cartProductsService = cartProductsService;
        }

        public IActionResult CheckOut()
        {
            var model = cartProductsService.GetAllProductsForTheUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            return View(model);
        }
        
        public IActionResult Remove(string productId)
        {
            cartProductsService.RemoveFromCart(User.FindFirstValue(ClaimTypes.NameIdentifier), productId);
            return Redirect("CheckOut");
        }

        public IActionResult Purchase()
        {
            cartProductsService.BuyProducts(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return Redirect("CheckOut");
        }

    }
}
