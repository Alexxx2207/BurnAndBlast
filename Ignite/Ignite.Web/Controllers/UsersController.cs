﻿using Ignite.Services.Users;
using Ignite.Web.Areas.Identity.Pages.Account.Manage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Ignite.Web.Areas.Identity.Pages.Account.Manage.IndexModel;

namespace Ignite.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUsersService usersService;

        public UsersController(
            ILogger<UsersController> logger,
            IUsersService usersService)
        {
            _logger = logger;
            this.usersService = usersService;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return this.Redirect("/Home/IndexLoggedIn");

            return this.LocalRedirect("/Identity/Account/Login");
        }
        
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return this.Redirect("/Home/IndexLoggedIn");

            return this.LocalRedirect("/Identity/Account/Register");

        }

        public IActionResult RemoveProfileImage(string userId)
        {
            usersService.RemoveImage(userId);

            return LocalRedirect("/Identity/Account/Manage");
        }
    }
}
