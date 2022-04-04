using Ignite.Data;
using Ignite.Models;
using Ignite.Models.Enums;
using Ignite.Models.ViewModels.Subscriptions;
using Ignite.Services.Subscriptions;
using Microsoft.EntityFrameworkCore;
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

            var sub = GetSubscriptionByGUID(subId);

            DateTime expiration = DateTime.Now + sub.Duration;

            db.UsersSubscriptions.Add(new UserSubscription
            {
                SubscriptionOrderId = Guid.NewGuid().ToString(),
                UserId = userId,
                SubscriptionId = subId,
                ExpirationDate = expiration
            });

            db.SaveChanges();
        }

        public bool CheckSubscriptionExists(string subId)
        {
            return db.Subscriptions.Any(s => subId == s.Guid && !s.IsDeleted);
        }

        public IEnumerable<string> GetAllPeopleEmailsWithPremiumAndVipSubs()
        {
            return db.UsersSubscriptions
                    .Include(us => us.Subscription)
                    .Where(us => us.ExpirationDate >= DateTime.Now &&
                    (us.Subscription.Type == SubscriptionType.Premium || us.Subscription.Type == SubscriptionType.VIP))
                    .Include(us => us.User)
                    .Select(us => us.User.Email)
                    .ToList()
                    .Distinct();
        }

        public List<AllSubscriptionsViewModel> GetAllSubscriptions(string userId)
        {
            var bestUserSubscriptionOrderInPageObject = GetBestNotExpiredSubscription(userId)?
                                                    .Subscription.OrderInPage;

            int bestUserSubscriptionOrderInPage = bestUserSubscriptionOrderInPageObject == null ?
                                                    -1 : bestUserSubscriptionOrderInPageObject.Value;

            return db.Subscriptions
                    .Where(s => !s.IsDeleted)
                    .Select(s => new AllSubscriptionsViewModel
                    {
                        Guid = s.Guid,
                        Name = s.Name,
                        Duration = s.Duration,
                        Type = s.Type,
                        OrderInPage = s.OrderInPage,
                        UserBestSubscriptionOrderInPage = bestUserSubscriptionOrderInPage
                    })
                    .ToList();
        }

        public UserSubscription GetBestNotExpiredSubscription(string userId)
        {
            return db.UsersSubscriptions
                        .Where(us => us.UserId == userId &&
                                us.ExpirationDate >= DateTime.Now)
                        .Include(us => us.Subscription)
                        .OrderByDescending(us => us.Subscription.OrderInPage)
                        .FirstOrDefault();
        }

        public Subscription GetSubscriptionByGUID(string subscriptionId)
        {
            if(!CheckSubscriptionExists(subscriptionId))
                throw new ArgumentException("Invalid data.");

            return db.Subscriptions.First(c => c.Guid == subscriptionId);
        }
    }
}
