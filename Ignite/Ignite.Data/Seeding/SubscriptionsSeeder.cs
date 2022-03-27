using Ignite.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Data.Seeding
{
    public class SubscriptionsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            await SeedSubscriptionAsync(dbContext,
                                        SubscriptionType.Daily.ToString(),
                                        SubscriptionType.Daily,
                                        GlobalConstants.GlobalConstants.SubscriptionTypeDailyPrice,
                                        GlobalConstants.GlobalConstants.SubscriptionTypeDailyDurationInDays);
            await SeedSubscriptionAsync(dbContext,
                                        SubscriptionType.Basic.ToString(),
                                        SubscriptionType.Basic,
                                        GlobalConstants.GlobalConstants.SubscriptionTypeBasicPrice,
                                        GlobalConstants.GlobalConstants.SubscriptionTypeBasicDurationInDays);
            await SeedSubscriptionAsync(dbContext,
                                        SubscriptionType.Premium.ToString(),
                                        SubscriptionType.Premium,
                                        GlobalConstants.GlobalConstants.SubscriptionTypePremiumPrice,
                                        GlobalConstants.GlobalConstants.SubscriptionTypePremiumDurationInDays);
            await SeedSubscriptionAsync(dbContext,
                                        SubscriptionType.VIP.ToString(),
                                        SubscriptionType.VIP,
                                        GlobalConstants.GlobalConstants.SubscriptionTypeVipPrice,
                                        GlobalConstants.GlobalConstants.SubscriptionTypeVipDurationInDays);

            dbContext.SaveChanges();
        }

        private static async Task SeedSubscriptionAsync(
            ApplicationDbContext dbContext, string subName, SubscriptionType type, decimal price, TimeSpan duration)
        {
            var guid = Guid.NewGuid().ToString();

            if (!dbContext.Subscriptions.Any(s => s.Name == subName))
            {
                if (dbContext.Products.Any(s => s.Name == subName &&
                                                 s.ProductType == ProductType.Subscription))
                {
                    guid = dbContext
                            .Products
                            .First(s => s.Name == subName && s.ProductType == ProductType.Subscription)
                            .Guid;
                }

                dbContext.Subscriptions.Add(new Models.Subscription
                { 
                    Guid = guid,
                    Name = subName, 
                    Duration = duration,
                    Type = type
                });
            }

            if(!dbContext.Products.Any(s => s.Name == subName &&
                s.ProductType == ProductType.Subscription))
            {
                if (dbContext.Subscriptions.Any(s => s.Name == subName))
                {
                    guid = dbContext
                            .Subscriptions
                            .First(s => s.Name == subName)
                            .Guid;
                }

                dbContext.Products.Add(new Models.Product
                {
                    Guid = guid,
                    Name = subName,
                    Price = price,
                    ProductType = ProductType.Subscription
                });
            }
        }
    }
}
