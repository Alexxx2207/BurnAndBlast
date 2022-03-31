using Ignite.Services.Classes;
using Ignite.Services.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ignite.Services.CartProducts;

namespace Ignite.Web.Controllers
{
    [Authorize]
    public class ClassesController : Controller
    {
        private readonly IClassesService classesService;
        private readonly ICartProductsService cartProductsService;

        public ClassesController(
            IClassesService classesService,
            ICartProductsService cartProductsService)
        {
            this.classesService = classesService;
            this.cartProductsService = cartProductsService;
        }

        public IActionResult All()
        {
            var model = classesService.GetAllClasses(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(model);
        }

        public IActionResult Details(string classId)
        {
            var model = classesService.GetDetailsOfClass(User.FindFirstValue(ClaimTypes.NameIdentifier), classId);

            return this.View(model);
        }
    }
}
