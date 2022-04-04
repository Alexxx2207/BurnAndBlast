using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Ignite.Data.Seeding;
using Moq;
using Microsoft.EntityFrameworkCore;
using Ignite.Data;
using Ignite.Models;
using Ignite.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace Ignite.UnitTests.Data
{
    public class SeedingUnitTests
    {
        [Fact]
        public void ApplicationDbContextSeederClassCheck_DbContextNull_ShouldFail()
        {
            var applicationDbContext = new ApplicationDbContextSeeder();

            var serviceProvider = new Mock<IServiceProvider>();

            var exception = Record.ExceptionAsync(() => applicationDbContext.SeedAsync(null, serviceProvider.Object));

            Assert.NotNull(exception);
        }

        [Fact]
        public void ApplicationDbContextSeederClassCheck_ServiceProviderNull_ShouldFail()
        {
            var applicationDbContext = new ApplicationDbContextSeeder();

            var serviceProvider = new Mock<IServiceProvider>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "ApplicationDbContextSeederClassCheck_ServiceProviderNull_ShouldFail")
              .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                var exception = Record.ExceptionAsync(() => applicationDbContext.SeedAsync(dbContext, null));
                Assert.NotNull(exception);
            }

        }

        [Fact]
        public void ApplicationDbContextSeederClassCheck_ShouldSucceed()
        {
            var applicationDbContext = new ApplicationDbContextSeeder();

            var serviceProvider = new Mock<IServiceProvider>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "ApplicationDbContextSeederClassCheck_ShouldSucceed")
              .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {

                var exception = Record.ExceptionAsync(() => applicationDbContext.SeedAsync(dbContext, serviceProvider.Object));
                Assert.NotNull(exception);
            }
        }

        [Fact]
        public void SubscriptionSeedersCheck_ShouldSucceed()
        {
            var subscriptionsSeeder = new SubscriptionsSeeder();

            var serviceProvider = new Mock<IServiceProvider>();


            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "SubscriptionSeedersCheck_ShouldSucceed")
              .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                subscriptionsSeeder.SeedAsync(dbContext, serviceProvider.Object);

                Assert.Equal(GlobalConstants.GlobalConstants.SubscriptionTypesCount, dbContext.Subscriptions.Count());
            }
        }
        
        [Fact]
        public void SubscriptionSeedersWithProductButWithoutSubscriptionCheck_ShouldSucceed()
        {
            var subscriptionsSeeder = new SubscriptionsSeeder();

            var serviceProvider = new Mock<IServiceProvider>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "SubscriptionSeedersWithProductButWithoutSubscriptionCheck_ShouldSucceed")
              .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Products.Add(new Product 
                {
                    Guid = Guid.NewGuid().ToString(),
                    Name = SubscriptionType.Daily.ToString(),
                    ProductType = ProductType.Subscription
                });

                dbContext.SaveChanges();

                subscriptionsSeeder.SeedAsync(dbContext, serviceProvider.Object);

                Assert.Equal(GlobalConstants.GlobalConstants.SubscriptionTypesCount, dbContext.Subscriptions.Count());
            }
        }
        
        [Fact]
        public void SubscriptionSeedersWithSubscriptionButWithoutProductCheck_ShouldSucceed()
        {
            var subscriptionsSeeder = new SubscriptionsSeeder();

            var serviceProvider = new Mock<IServiceProvider>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "SubscriptionSeedersWithSubscriptionButWithoutProductCheck_ShouldSucceed")
              .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Subscriptions.Add(new Subscription 
                {
                    Guid = Guid.NewGuid().ToString(),
                    Name = SubscriptionType.Daily.ToString(),
                    Type = SubscriptionType.Daily
                });

                dbContext.SaveChanges();

                subscriptionsSeeder.SeedAsync(dbContext, serviceProvider.Object);

                Assert.Equal(GlobalConstants.GlobalConstants.SubscriptionTypesCount, dbContext.Subscriptions.Count());
            }
        }
    }
}
