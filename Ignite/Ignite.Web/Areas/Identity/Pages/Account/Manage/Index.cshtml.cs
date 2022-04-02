// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Ignite.Data;
using Ignite.Models;
using Ignite.Models.ViewModels.Subscriptions;
using Ignite.Services.Subscriptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ignite.Web.Areas.Identity.Pages.Account.Manage
{

    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext db;
        private readonly ISubscriptionsService subscriptionsService;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext db,
            ISubscriptionsService subscriptionsService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.db = db;
            this.subscriptionsService = subscriptionsService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        [BindProperties]
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            public ApplicationUser ApplicationUser { get; set; }

            public UserSubscriptionsViewModel Subscription { get; set; }

            public IFormFile Image { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Input = new InputModel
            {
                ApplicationUser = user
            };

            var userSubscription = subscriptionsService
                        .GetBestNotExpiredSubscription(user.Id);

            if (userSubscription != null)
            {
                Input.Subscription = new UserSubscriptionsViewModel
                {
                    ExpirationDate = userSubscription.ExpirationDate,
                    Name = userSubscription.Subscription.Name,
                };
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (Input.Image != null && Input.Image.Length > 0)
            {
                var imageName =  "d" + DateTime.UtcNow.ToString()
                                        .Replace("/", "")
                                        .Replace(" ", "")
                                        .Replace(":", "") 
                                        + Input.Image.FileName
                                        .ToLower();

                using (var stream = new FileStream($"wwwroot/Profile Pics/{imageName}", FileMode.Create))
                {
                    await Input.Image.CopyToAsync(stream);


                    await _signInManager.RefreshSignInAsync(user);
                }
                var userDB = db.Users.Find(user.Id);

                userDB.ProfilePicture = imageName;
                db.SaveChanges();
            }
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
