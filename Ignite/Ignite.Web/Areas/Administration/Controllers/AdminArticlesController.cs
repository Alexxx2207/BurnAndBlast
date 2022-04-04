using Ignite.Data;
using Ignite.Models;
using Ignite.Models.InputModels.Articles;
using Ignite.Models.ParentModels;
using Ignite.Models.ViewModels.Articles;
using Ignite.Services.Articles;
using Ignite.Services.Subscriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ignite.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Area("Administration")]
    public class AdminArticlesController : Controller
    {
        private readonly ISubscriptionsService subscriptionsService;
        private readonly IActicleService acticleService;
        private readonly IConfiguration config;

        public AdminArticlesController(
            ISubscriptionsService subscriptionsService,
            IActicleService acticleService,
            IConfiguration config)
        {
            this.subscriptionsService = subscriptionsService;
            this.acticleService = acticleService;
            this.config = config;
        }

        public IActionResult AllArticles()
        {
            var model = new AllArticlesParentModel()
            {
                InputModel = new AddArticleInputModel(),
                ViewModel = acticleService.GetAllArticles()
            };
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddArticle(AllArticlesParentModel model)
        {
            ModelState.Remove("ViewModel");

            if (!ModelState.IsValid)
            {
                var parentModel = new AllArticlesParentModel()
                {
                    InputModel = model.InputModel,
                    ViewModel = acticleService.GetAllArticles()
                };

                return View("AllArticles", parentModel);

            }

            acticleService.AddArticle(model.InputModel);

            return Redirect("AllArticles");
        }

        public IActionResult RemoveArticle(string articleId)
        {
            acticleService.RemoveArticle(articleId);

            return Redirect("AllArticles");
        }


        public IActionResult ChangeArticle(string articleId)
        {
            var a = acticleService.GetArticleByGUID(articleId);

            var model = new ChangeArticleParentModel
            {
                InputModel = new ChangeArticleInputModel(),
                ViewModel = new AllArticlesViewModel
                {
                    Guid = articleId,
                    Title = a.Title,
                    Text = a.Text,
                    Link = a.Link,
                }
            };

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult ChangeArticle(ChangeArticleParentModel model)
        {
            ModelState.Remove("ViewModel");

            if (!ModelState.IsValid)
            {
                var a = acticleService.GetArticleByGUID(model.InputModel.Guid);

                model.ViewModel = new AllArticlesViewModel
                {
                    Guid = model.InputModel.Guid,
                    Title = a.Title,
                    Text = a.Text,
                    Link = a.Link,
                };

                return View("ChangeArticle", model);
            }

            acticleService.ChangeArticle(model.InputModel);


            return Redirect("/Administration/AdminArticles/AllArticles");
        }

        public async Task<IActionResult> SendEmails(string articleId)
        {
            var peopleMails = subscriptionsService.GetAllPeopleEmailsWithPremiumAndVipSubs();

            AllArticlesViewModel article = null;

            try
            {
                article = acticleService.GetArticleByGUID(articleId);
            }
            catch (Exception)
            {
                return Redirect("AllArticles");
            }

            var articleBody = $"<h3>{article.Text}</h3>" +
                @" <br /> " +
                $"<h3>Link: {article.Link}<h3>";

            var apiKey = config["SENDGRID_API_KEY"];
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress($"{GlobalConstants.GlobalConstants.FromEmail}", $"{GlobalConstants.GlobalConstants.EmailSenderName}"),
                Subject = article.Title,
                HtmlContent = articleBody,
            };

            foreach (var email in peopleMails)
            {
                msg.AddTo(new EmailAddress($"{email}", "Customer"));
                await client.SendEmailAsync(msg);
            }

            if (peopleMails.Count() > 0)
                acticleService.RemoveArticle(article.Guid);

            return Redirect("AllArticles");
        }
    }
}
