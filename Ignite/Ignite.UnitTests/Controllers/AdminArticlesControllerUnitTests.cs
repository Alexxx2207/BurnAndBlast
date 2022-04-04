using Microsoft.Extensions.Configuration;
using Ignite.Models.ViewModels.Articles;
using Ignite.Services.Articles;
using Ignite.Services.Subscriptions;
using Ignite.Web.Areas.Administration.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Ignite.Models.ParentModels;
using Ignite.Models.InputModels.Articles;

namespace Ignite.UnitTests.Controllers
{
    public class AdminArticlesControllerUnitTests
    {
         [Fact]
        public void AllArticleReturnLocalRedirect()
        {
            var articlesService = new Mock<IActicleService>();
            var subscriptionsService = new Mock<ISubscriptionsService>();
            var configuration = new Mock<IConfiguration>();

            var adminArticlesController = new AdminArticlesController(
                        subscriptionsService.Object,
                        articlesService.Object,
                        configuration.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            adminArticlesController.ControllerContext = context;

            articlesService.Setup(c => c.GetAllArticles())
                .Returns(new List<AllArticlesViewModel>());

            var result = adminArticlesController.AllArticles();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<AllArticlesParentModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void AddArticlesReturnLocalRedirect()
        {
            var articlesService = new Mock<IActicleService>();
            var subscriptionsService = new Mock<ISubscriptionsService>();
            var configuration = new Mock<IConfiguration>();

            var adminArticlesController = new AdminArticlesController(
                        subscriptionsService.Object,
                        articlesService.Object,
                        configuration.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            adminArticlesController.ControllerContext = context;

            AllArticlesParentModel model = new AllArticlesParentModel()
            {
                InputModel = new AddArticleInputModel()
                {
                    Text = "text",
                    Title = "text",
                    Link = "text"
                }
            };

            var result = adminArticlesController.AddArticle(model);

            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public void AddArticleThrowsErrorName()
        {
            var articlesService = new Mock<IActicleService>();
            var subscriptionsService = new Mock<ISubscriptionsService>();
            var configuration = new Mock<IConfiguration>();

            var adminArticlesController = new AdminArticlesController(
                        subscriptionsService.Object,
                        articlesService.Object,
                        configuration.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            adminArticlesController.ControllerContext = context;

            AllArticlesParentModel model = new AllArticlesParentModel()
            {
                InputModel = new AddArticleInputModel()
                {
                    Link = "text"
                }
            };

            adminArticlesController.ModelState.AddModelError("error", "error");

            var result = adminArticlesController.AddArticle(model);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void RemoveArticleReturnRedirectsCheck()
        {
            var articlesService = new Mock<IActicleService>();
            var subscriptionsService = new Mock<ISubscriptionsService>();
            var configuration = new Mock<IConfiguration>();

            var adminArticlesController = new AdminArticlesController(
                        subscriptionsService.Object,
                        articlesService.Object,
                        configuration.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            adminArticlesController.ControllerContext = context;

            string articleId = Guid.NewGuid().ToString();

            articlesService.Setup(c => c.CheckArticleExists(articleId))
                .Returns(true);

            var result = adminArticlesController.RemoveArticle(articleId);

            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public void ChangeArticleReturnViewCheck()
        {
            var articlesService = new Mock<IActicleService>();
            var subscriptionsService = new Mock<ISubscriptionsService>();
            var configuration = new Mock<IConfiguration>();

            var adminArticlesController = new AdminArticlesController(
                        subscriptionsService.Object,
                        articlesService.Object,
                        configuration.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            adminArticlesController.ControllerContext = context;


            string articleId = Guid.NewGuid().ToString();

            articlesService.Setup(c => c.GetArticleByGUID(articleId))
                .Returns(new AllArticlesViewModel());

            var result = adminArticlesController.ChangeArticle(articleId);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ChangeArticleParentModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void ChangeArticleReturnRedirectCheck()
        {
            var articlesService = new Mock<IActicleService>();
            var subscriptionsService = new Mock<ISubscriptionsService>();
            var configuration = new Mock<IConfiguration>();

            var adminArticlesController = new AdminArticlesController(
                        subscriptionsService.Object,
                        articlesService.Object,
                        configuration.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            adminArticlesController.ControllerContext = context;


            string articleId = Guid.NewGuid().ToString();

            articlesService.Setup(c => c.GetArticleByGUID(articleId))
                .Returns(new AllArticlesViewModel());

            ChangeArticleParentModel model = new ChangeArticleParentModel
            {
                InputModel = new ChangeArticleInputModel
                {
                    Guid = articleId,
                }
            };

            var result = adminArticlesController.ChangeArticle(model);

            Assert.IsType<RedirectResult>(result);

            adminArticlesController.ModelState.AddModelError("error", "error");

            result = adminArticlesController.ChangeArticle(model);

            Assert.IsType<ViewResult>(result);
        }
        
        [Fact]
        public void SendEmailReturnRedirectCheck()
        {
            var articlesService = new Mock<IActicleService>();
            var subscriptionsService = new Mock<ISubscriptionsService>();
            var configuration = new Mock<IConfiguration>();

            var adminArticlesController = new AdminArticlesController(
                        subscriptionsService.Object,
                        articlesService.Object,
                        configuration.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            adminArticlesController.ControllerContext = context;


            string articleId = Guid.NewGuid().ToString();

            articlesService.Setup(c => c.GetArticleByGUID(articleId))
                .Throws(new ArgumentException());

            var result = adminArticlesController.SendEmails(articleId);

            Assert.IsAssignableFrom<Task<IActionResult>>(result);



            subscriptionsService.Setup(c => c.GetAllPeopleEmailsWithPremiumAndVipSubs())
                .Returns(new string[2] { "sw@sw.sw", "as@as.as" });

            articlesService.Setup(c => c.GetArticleByGUID(articleId))
                .Returns(new AllArticlesViewModel());

            configuration.Setup(c => c["SENDGRID_API_KEY"])
                .Returns("secret");

            result = adminArticlesController.SendEmails(articleId);

            Assert.IsAssignableFrom<Task<IActionResult>>(result);
        }
    }
}
