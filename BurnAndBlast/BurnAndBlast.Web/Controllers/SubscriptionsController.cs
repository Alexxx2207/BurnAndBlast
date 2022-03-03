using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BurnAndBlast.Web.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly ILogger<SubscriptionsController> _logger;

        public SubscriptionsController(ILogger<SubscriptionsController> logger)
        {
            _logger = logger;
        }

        public IActionResult All()
        {
            return View();
        }

        //When clicking a button Purchase
        [Authorize]
        public IActionResult Purchase(int subscriptionType)
        {
            //...
            return Redirect("/Subscriptions/CheckOut");
        }

        [Authorize]
        public IActionResult CheckOut()
        {
            return View();
        }
    }
}
