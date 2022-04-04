using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Models.InputModels.Articles
{
    public class ChangeArticleInputModel
    {
        public string Guid { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Text { get; set; }

        [Required]
        public string Link { get; set; }
    }
}
