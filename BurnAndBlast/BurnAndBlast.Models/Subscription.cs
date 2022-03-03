namespace BurnAndBlast.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using BurnAndBlast.Data.Models.Enums;

    public class Subscription
    {
        public Subscription()
        {
            this.UsersSubscriptions = new HashSet<UserSubscription>();
        }

        [Key]
        public string Guid { get; set; }

        public SubscriptionType Type { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<UserSubscription> UsersSubscriptions { get; set; }
    }
}
