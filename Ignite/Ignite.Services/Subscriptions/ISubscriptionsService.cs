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

        List<AllSubscriptionsViewModel> GetAllSubscriptions();

        bool CheckSubscriptionExists(string subId);

        void AddSubscriptionToUser(string userId, string classId);

    }
}
