using Ignite.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Models.ViewModels.Subscriptions
{
    public class AllSubscriptionsViewModel
    {
        public string Guid { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public SubscriptionType Type { get; set; }

        public TimeSpan Duration { get; set; }

        public int OrderInPage { get; set; }

        public int UserBestSubscriptionOrderInPage { get; set; }
    }
}
