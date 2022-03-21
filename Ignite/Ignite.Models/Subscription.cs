namespace Ignite.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Ignite.Models.Enums;

    public class Subscription
    {
        [Key]
        public string Guid { get; set; }

        public SubscriptionType Type { get; set; }

        public decimal Price { get; set; }

        public bool IsDeleted { get; set; }

    }
}
