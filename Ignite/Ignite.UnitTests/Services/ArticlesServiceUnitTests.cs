using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ignite.Data;
using Ignite.Models;
using Ignite.Models.InputModels.Articles;
using Ignite.Services.Articles;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Ignite.UnitTests.Services
{
    public class ArticlesServiceUnitTests
    {
        [Fact]
        public void AddArticle_ShoudPass()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "AddArticle_ShoudPass")
            .Options;

            AddArticleInputModel model = new AddArticleInputModel
            {
                Text = "text",
                Title = "text",
                Link = "text"
            };

            var expectedCount = 1;
            var actual = 0;

            using (var dbContext = new ApplicationDbContext(options))
            {
                var articlesService = new ArticleService(dbContext);
                articlesService.AddArticle(model);

                actual = dbContext.Articles.Count();
            }

            Assert.Equal(expectedCount, actual);
        }

        [Fact]
        public void GetAllArticles_ShouldPass()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "GetAllArticles_ShouldPass")
              .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Articles.Add(new Article
                {
                    Guid = Guid.NewGuid().ToString(),
                    Text = "text",
                    Title = "text",
                    Link = "text"
                });

                dbContext.Articles.Add(new Article
                {
                    Guid = Guid.NewGuid().ToString(),
                    Text = "text",
                    Title = "text",
                    Link = "text"
                });

                dbContext.SaveChanges();

                var articlesService = new ArticleService(dbContext);

                Assert.Equal(2, articlesService.GetAllArticles().Count());
            }
        }

        [Fact]
        public void GetArticleByGUID_ShouldFail()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "GetArticleByGUID_ShouldFail")
                 .Options;

            var articleId = Guid.NewGuid().ToString();
            var articleIdDifferent = Guid.NewGuid().ToString();

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Articles.Add(new Article
                {
                    Guid = Guid.NewGuid().ToString(),
                    Text = "text",
                    Title = "text",
                    Link = "text"
                });

                dbContext.SaveChanges();

                var articlesService = new ArticleService(dbContext);

                Assert.Throws<ArgumentException>(() => articlesService.GetArticleByGUID(articleIdDifferent));
            }
        }

        [Fact]
        public void RemoveArticle_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "RemoveArticle_ShouldSucceed")
                 .Options;

            var articleId = Guid.NewGuid().ToString();

            var expectedInDB = 1;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Articles.Add(new Article
                {
                    Guid = articleId,
                    Text = "text",
                    Title = "text",
                    Link = "text"
                });

                dbContext.SaveChanges();

                var articlesService = new ArticleService(dbContext);

                articlesService.RemoveArticle(articleId);

                Assert.Equal(expectedInDB, dbContext.Articles.Count());
                Assert.Empty(articlesService.GetAllArticles());
            }
        }

        [Fact]
        public void ChangeArticle_ShouldPass()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(databaseName: "ChangeArticle_ShouldPass")
           .Options;

            var articleId = Guid.NewGuid().ToString();

            var model = new ChangeArticleInputModel
            {
                Guid = articleId,
                Text = "text",
                Title = "text",
                Link = "text"
            };

            var actualArticle = new Article();

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Articles.Add(new Article
                {
                    Guid = articleId,
                    Text = "text",
                    Title = "text",
                    Link = "text"
                });

                dbContext.SaveChanges();

                var articlesService = new ArticleService(dbContext);

                articlesService.ChangeArticle(model);

                actualArticle = dbContext.Find<Article>(articleId);
            }

            Assert.True(actualArticle.Title == model.Title &&
                        actualArticle.Text == model.Text &&
                        actualArticle.Link == model.Link);
        }
    }
}
