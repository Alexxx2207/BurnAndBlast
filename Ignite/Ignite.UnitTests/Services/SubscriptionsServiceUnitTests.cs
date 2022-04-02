using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ignite.Data;
using Ignite.Models;
using Ignite.Services.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Ignite.UnitTests.Services
{
    public class SubscriptionsServiceUnitTests
    {
        [Fact]
        public void AddSubscriptionToUser_ShouldFail()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AddSubscriptionToUser_ShouldFail")
                .Options;

            var userId = Guid.NewGuid().ToString();
            var subscriptionId = Guid.NewGuid().ToString();

            using (var dbContext = new ApplicationDbContext(options))
            {
                var subscriptionService = new SubscriptionsService(dbContext);

                Assert.Throws<ArgumentException>(() => subscriptionService.AddSubscriptionToUser(userId, subscriptionId));
            }
        }

        [Fact]
        public void AddSubscriptionToUser_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AddSubscriptionToUser_ShouldSucceed")
                .Options;

            var userId = Guid.NewGuid().ToString();
            var subscriptionId = Guid.NewGuid().ToString();

            var expectedCountInDB = 1;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Users.Add(new ApplicationUser
                { 
                    Id = userId,
                    FirstName = "FirstName",
                    LastName = "LastName",
                });

                dbContext.Subscriptions.Add(new Subscription 
                {
                    Guid = subscriptionId,
                    Duration = new TimeSpan(100),
                    Name = "name",
                    OrderInPage = 1,
                    Type = Models.Enums.SubscriptionType.Daily,
                    IsDeleted = false
                });

                dbContext.SaveChanges();

                var subscriptionService = new SubscriptionsService(dbContext);

                subscriptionService.AddSubscriptionToUser(userId, subscriptionId);

                Assert.Equal(expectedCountInDB, dbContext.UsersSubscriptions.Count());
            }
        }

        [Fact]
        public void CheckSubscriptionExists_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                   .UseInMemoryDatabase(databaseName: "CheckSubscriptionExists_ShouldSucceed")
                   .Options;

            var subscriptionId = Guid.NewGuid().ToString();
            var subscriptionIdDifferent = Guid.NewGuid().ToString();

            using (var dbContext = new ApplicationDbContext(options))
            {

                dbContext.Subscriptions.Add(new Subscription
                {
                    Guid = subscriptionId,
                    Duration = new TimeSpan(100),
                    Name = "name",
                    OrderInPage = 1,
                    Type = Models.Enums.SubscriptionType.Daily,
                    IsDeleted = false
                });

                dbContext.SaveChanges();

                var subscriptionService = new SubscriptionsService(dbContext);

                Assert.True(subscriptionService.CheckSubscriptionExists(subscriptionId));
                Assert.True(!subscriptionService.CheckSubscriptionExists(subscriptionIdDifferent));
            }
        }

        [Fact]
        public void GetAllSubscriptions_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "GetAllSubscriptions_ShouldSucceed")
                 .Options;

            var userId = Guid.NewGuid().ToString();
            var subscriptionId = Guid.NewGuid().ToString();

            var expectedCount = 2;

            var subscription = new Subscription
            {
                Guid = subscriptionId,
                Duration = new TimeSpan(100),
                Name = "name",
                OrderInPage = 1,
                Type = Models.Enums.SubscriptionType.Daily,
                IsDeleted = false
            };

            using (var dbContext = new ApplicationDbContext(options))
            {

                dbContext.Subscriptions.Add(subscription);

                dbContext.Subscriptions.Add(new Subscription
                {
                    Guid = Guid.NewGuid().ToString(),
                    Duration = new TimeSpan(100),
                    Name = "name",
                    OrderInPage = 1,
                    Type = Models.Enums.SubscriptionType.Daily,
                    IsDeleted = false
                });

                dbContext.UsersSubscriptions.Add(new UserSubscription
                { 
                    SubscriptionOrderId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    SubscriptionId = subscriptionId,
                    ExpirationDate = DateTime.Now.AddDays(2),
                    Subscription = subscription
                });

                dbContext.SaveChanges();

                var subscriptionService = new SubscriptionsService(dbContext);

                Assert.Equal(expectedCount, subscriptionService.GetAllSubscriptions(userId).Count());
            }
        }
        
        [Fact]
        public void GetAllSubscriptions_CantGetBestNotExpiredSubscription_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "GetAllSubscriptions_CantGetBestNotExpiredSubscription_ShouldSucceed")
                 .Options;

            var expectedCount = 2; 

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Subscriptions.Add(new Subscription
                {
                    Guid = Guid.NewGuid().ToString(),
                    Duration = new TimeSpan(100),
                    Name = "name",
                    OrderInPage = 1,
                    Type = Models.Enums.SubscriptionType.Daily,
                    IsDeleted = false
                });

                dbContext.Subscriptions.Add(new Subscription
                {
                    Guid = Guid.NewGuid().ToString(),
                    Duration = new TimeSpan(100),
                    Name = "name",
                    OrderInPage = 1,
                    Type = Models.Enums.SubscriptionType.Daily,
                    IsDeleted = false
                });

                dbContext.SaveChanges();

                var subscriptionService = new SubscriptionsService(dbContext);

                Assert.Equal(expectedCount, subscriptionService.GetAllSubscriptions(It.IsAny<string>()).Count());
            }
        }

        [Fact]
        public void GetSubscriptionByGUID_ShouldFail()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetSubscriptionByGUID_ShouldFail")
                .Options;

            var subscriptionId = Guid.NewGuid().ToString();

            using (var dbContext = new ApplicationDbContext(options))
            {
                var subscriptionService = new SubscriptionsService(dbContext);

                Assert.Throws<ArgumentException>(() => subscriptionService.GetSubscriptionByGUID(subscriptionId));
            }
        }

        [Fact]
        public void GetSubscriptionByGUID_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AddSubscriptionToUser_ShouldSucceed")
                .Options;

            var subscriptionId = Guid.NewGuid().ToString();

            var expected = new Subscription
            {
                Guid = subscriptionId,
                Duration = new TimeSpan(100),
                Name = "name",
                OrderInPage = 1,
                Type = Models.Enums.SubscriptionType.Daily,
                IsDeleted = false
            };

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Subscriptions.Add(expected);

                dbContext.SaveChanges();

                var subscriptionService = new SubscriptionsService(dbContext);

                var actual = subscriptionService.GetSubscriptionByGUID(subscriptionId);

                Assert.True(expected.Name == actual.Name &&
                            expected.Duration == actual.Duration &&
                            expected.OrderInPage == actual.OrderInPage &&
                            expected.Type == actual.Type);
            }
        }
    }
}
