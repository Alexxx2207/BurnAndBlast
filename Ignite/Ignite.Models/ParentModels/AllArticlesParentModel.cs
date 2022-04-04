using Ignite.Models.InputModels.Articles;
using Ignite.Models.ViewModels.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Models.ParentModels
{
    public class AllArticlesParentModel
    {
        public AddArticleInputModel InputModel { get; set; }

        public List<AllArticlesViewModel> ViewModel { get; set; }
    }
}
