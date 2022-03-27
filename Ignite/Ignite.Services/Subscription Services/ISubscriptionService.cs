using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ignite.Models;

namespace Ignite.Services.Subscription_Services
{
    public interface ISubscriptionService
    {
        Subscription GetSubscriptionByGUID(string subscriptionId);
    }
}
