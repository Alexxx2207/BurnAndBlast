using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ignite.Models;
using Ignite.Models.ViewModels.Subscriptions;

namespace Ignite.Services.Subscriptions
{
    public interface ISubscriptionsService
    {
        Subscription GetSubscriptionByGUID(string subscriptionId);

        List<AllSubscriptionsViewModel> GetAllSubscriptions(string userId);

        bool CheckSubscriptionExists(string subId);

        void AddSubscriptionToUser(string userId, string classId);

        UserSubscription GetBestNotExpiredSubscription(string userId);

        IEnumerable<string> GetAllPeopleEmailsWithPremiumAndVipSubs();

    }
}
