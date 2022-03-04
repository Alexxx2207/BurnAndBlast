using System.ComponentModel.DataAnnotations;

namespace Ignite.Models
{
    public class Article
    {
        [Key]
        public string Guid { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string Link { get; set; }
    }
}
