using System.ComponentModel.DataAnnotations;

namespace BurnAndBlast.Data.Models
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
