using Ignite.Data;
using Ignite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Services.Subscription_Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ApplicationDbContext db;

        public SubscriptionService(ApplicationDbContext db)
        {
            this.db = db;
        }
        public Subscription GetSubscriptionByGUID(string subscriptionId)
        {
            return db.Subscriptions.Find(subscriptionId);
        }
    }
}
