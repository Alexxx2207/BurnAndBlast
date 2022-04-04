using Ignite.Models.InputModels.Articles;
using Ignite.Models.ViewModels.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Models.ParentModels
{
    public class ChangeArticleParentModel
    {
        public ChangeArticleInputModel InputModel { get; set; }

        public AllArticlesViewModel ViewModel { get; set; }
    }
}
