using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Models.ViewModels.Articles
{
    public class AllArticlesViewModel
    {
        public string Guid { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }
        
        public string Link { get; set; }
    }
}
