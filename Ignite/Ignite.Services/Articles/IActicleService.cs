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
    public interface IActicleService
    {

        void AddArticle(AddArticleInputModel model);

        void RemoveArticle(string articleId);

        List<AllArticlesViewModel> GetAllArticles();

        AllArticlesViewModel GetArticleByGUID(string articleId);

        public bool CheckArticleExists(string articleId);

        void ChangeArticle(ChangeArticleInputModel model);

    }
}
