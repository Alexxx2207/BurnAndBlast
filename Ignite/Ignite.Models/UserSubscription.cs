namespace Ignite.Models
{
    using System;

    public class UserSubscription
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string SubscriptionId { get; set; }

        public virtual Subscription Subscription { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
