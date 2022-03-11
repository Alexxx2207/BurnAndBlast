namespace Ignite.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Ignite.Models.Enums;

    public class Subscription
    {
        public Subscription()
        {
            UsersSubscriptions = new HashSet<UserSubscription>();
        }

        [Key]
        public string Guid { get; set; }

        public SubscriptionType Type { get; set; }

        public decimal Price { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<UserSubscription> UsersSubscriptions { get; set; }
    }
}
