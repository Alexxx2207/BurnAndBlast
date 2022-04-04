using Ignite.Data;
using Ignite.Models;
using Ignite.Models.InputModels.Articles;
using Ignite.Models.ViewModels.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Services.Articles
{
    public class ArticleService : IActicleService
    {
        private readonly ApplicationDbContext db;

        public ArticleService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddArticle(AddArticleInputModel model)
        {
            var a = new Article()
            {
                Guid = Guid.NewGuid().ToString(),
                Title = model.Title,
                Text = model.Text,
                Link = model.Link,
            };

            db.Articles.Add(a);
            db.SaveChanges();
        }

        public List<AllArticlesViewModel> GetAllArticles()
        {
            return db.Articles
                .Where(a => !a.IsDeleted)
                .Select(x => new AllArticlesViewModel
                {
                    Guid = x.Guid,
                    Title = x.Title,
                    Text = x.Text,
                    Link = x.Link
                })
                .ToList();
        }

        public AllArticlesViewModel GetArticleByGUID(string articleId)
        {
            if (!CheckArticleExists(articleId))
                throw new ArgumentException("Invalid data.");

            var article = db.Articles.Find(articleId);

            return new AllArticlesViewModel 
                    {
                        Guid = articleId,
                        Title = article.Title,
                        Text = article.Text,
                        Link = article.Link,
                    };
        }

        public bool CheckArticleExists(string articleId)
        {
            return db.Articles.Any(x => x.Guid == articleId);
        }

        public void RemoveArticle(string articleId)
        {
            db.Articles.Find(GetArticleByGUID(articleId).Guid).IsDeleted = true;
            db.SaveChanges();
        }

        public void ChangeArticle(ChangeArticleInputModel model)
        {
            var a = db.Articles.Find(GetArticleByGUID(model.Guid).Guid);

            a.Title = model.Title;
            a.Link = model.Link;
            if (model.Text != null)
                a.Text = model.Text;

            db.SaveChanges();
        }
    }
}
