namespace Ignite.Models
{
    public class UserClass
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string ClassId { get; set; }

        public virtual Class Class { get; set; }
    }
}
