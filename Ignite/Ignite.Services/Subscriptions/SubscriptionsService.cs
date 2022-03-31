using Ignite.Data;
using Ignite.Models;
using Ignite.Models.ViewModels.Subscriptions;
using Ignite.Services.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Services.Subscriptions
{
    public class SubscriptionsService : ISubscriptionsService
    {
        private readonly ApplicationDbContext db;

        public SubscriptionsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddSubscriptionToUser(string userId, string subId)
        {
            if (!db.Users.Any(u => u.Id == userId) || !CheckSubscriptionExists(subId))
            {
                throw new ArgumentException("Invalid data.");
            }

            db.UsersSubscriptions.Add(new UserSubscription
            {
                SubscriptionOrderId = Guid.NewGuid().ToString(),
                UserId = userId,
                SubscriptionId = subId
            });

            db.SaveChanges();
        }

        public bool CheckSubscriptionExists(string subId)
        {
            return db.Subscriptions.Any(s => subId == s.Guid && !s.IsDeleted);
        }

        public List<AllSubscriptionsViewModel> GetAllSubscriptions()
        {
            return db.Subscriptions
                    .Where(s => !s.IsDeleted)
                    .Select(s => new AllSubscriptionsViewModel
                    {
                        Guid = s.Guid,
                        Name = s.Name,
                        Duration = s.Duration,
                        Type = s.Type,
                        OrderInPage = s.OrderInPage
                    })
                    .ToList();
        }

        public Subscription GetSubscriptionByGUID(string subscriptionId)
        {
            if(!CheckSubscriptionExists(subscriptionId))
                throw new ArgumentException("Invalid data.");

            return db.Subscriptions.First(c => c.Guid == subscriptionId);
        }
    }
}
