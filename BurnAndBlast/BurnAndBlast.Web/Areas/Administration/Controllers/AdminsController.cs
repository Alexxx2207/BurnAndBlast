using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BurnAndBlast.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminsController : Controller
    {
        private readonly ILogger<AdminsController> _logger;

        public AdminsController(ILogger<AdminsController> logger)
        {
            _logger = logger;
        }

        public IActionResult AddFitness()
        {
            return View();
        }
        
        public IActionResult AddEvent()
        {
            return View();
        }
        
        public IActionResult AddClass()
        {
            return View();
        }
    }
}
