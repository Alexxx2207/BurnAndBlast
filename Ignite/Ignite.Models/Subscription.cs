namespace Ignite.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Ignite.Models.Enums;

    public class Subscription
    {
        [Key]
        public string Guid { get; set; }

        public string Name { get; set; }

        public SubscriptionType Type { get; set; }

        public TimeSpan Duration { get; set; }

        public bool IsDeleted { get; set; }

        public int OrderInPage { get; set; }
    }
}
