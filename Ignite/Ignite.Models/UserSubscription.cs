using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Models
{
    public class UserSubscription
    {
        [Key]
        public string SubscriptionOrderId { get; set; }
        
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string SubscriptionId { get; set; }

        public Subscription Subscription { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
