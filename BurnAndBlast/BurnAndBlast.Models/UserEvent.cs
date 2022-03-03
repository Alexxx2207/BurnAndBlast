namespace BurnAndBlast.Data.Models
{
    public class UserEvent
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string EventId { get; set; }

        public virtual Event Event { get; set; }
    }
}
