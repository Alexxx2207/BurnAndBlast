using Ignite.Data;
using Ignite.Models;
using Ignite.Models.Enums;
using Ignite.Services.CartProducts;
using Ignite.Services.Classes;
using Ignite.Services.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace Ignite.UnitTests.Services
{
    public class CartProductsServiceUnitTests
    {
        [Fact]
        public void AddToCart_ShouldFail()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AddToCart_ShouldFail")
                .Options;

            var userId = Guid.NewGuid().ToString();
            var productId = Guid.NewGuid().ToString();
            var productTypeClass = ProductType.Class;
            var productTypeSubscription = ProductType.Subscription;

            var classesServiceMock = new Mock<IClassesService>();
            var subscriptionsServceMock = new Mock<ISubscriptionsService>();

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.UsersProducts.Add(new UserProduct
                {
                    OrderItemId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ProductId = productId,
                    IsInCart = true
                });
                dbContext.SaveChanges();

                var cartProductService = new CartProductsService(
                    dbContext,
                    classesServiceMock.Object,
                    subscriptionsServceMock.Object);

                Assert.Throws<ArgumentException>(() =>
                {
                    cartProductService.AddToCart(userId, productTypeClass, productId);
                });

                Assert.Throws<ArgumentException>(() =>
                {
                    cartProductService.AddToCart(userId, productTypeSubscription, productId);
                });
            }
        }
        
        [Fact]
        public void AddToCart_NoMoreSeats_ShouldFail()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AddToCart_NoMoreSeats_ShouldFail")
                .Options;

            var userId = Guid.NewGuid().ToString();
            var productId = Guid.NewGuid().ToString();
            var productTypeClass = ProductType.Class;
            var productTypeSubscription = ProductType.Subscription;

            var classesServiceMock = new Mock<IClassesService>();
            var subscriptionsServceMock = new Mock<ISubscriptionsService>();

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Classes.Add(new Class
                {
                    Guid = productId,
                    Description = "description",
                    Address = "address",
                    Name = "name",
                    DurationInMinutes = 60,
                    StartingDateTime = DateTime.Now,
                    AllSeats = 0
                });

                dbContext.SaveChanges();

                var cartProductService = new CartProductsService(
                    dbContext,
                    classesServiceMock.Object,
                    subscriptionsServceMock.Object);

                Assert.Throws<ArgumentException>(() =>
                {
                    cartProductService.AddToCart(userId, productTypeClass, productId);
                });
            }
        }

        [Fact]
        public void AddToCart_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "AddToCart_ShouldSucceed")
               .Options;

            var userId = Guid.NewGuid().ToString();
            var productId = Guid.NewGuid().ToString();
            var productTypeClass = ProductType.Class;
            var productTypeSubscription = ProductType.Subscription;

            var classesServiceMock = new Mock<IClassesService>();
            var subscriptionsServiceMock = new Mock<ISubscriptionsService>();

            var userIdForNewClass = Guid.NewGuid().ToString();
            var productIdForNewClass = Guid.NewGuid().ToString();

            var userIdForNewSubscription = Guid.NewGuid().ToString();
            var productIdForNewSubscription = Guid.NewGuid().ToString();
            var subscription = new Subscription
            {
                Guid = productIdForNewSubscription,
                Duration = new TimeSpan(100),
                Name = "nothing",
                IsDeleted = false,
                OrderInPage = 1,
                Type = SubscriptionType.Daily
                
            };

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Classes.Add(new Class
                {
                    Guid = productIdForNewClass,
                    Description = "description",
                    Address = "address",
                    Name = "name",
                    DurationInMinutes = 60,
                    StartingDateTime = DateTime.Now,
                    AllSeats = 5
                });

                dbContext.UsersProducts.Add(new UserProduct
                {
                    OrderItemId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ProductId = productId,
                    IsInCart = true,
                });

                dbContext.Subscriptions.Add(subscription);

                dbContext.SaveChanges();

                subscriptionsServiceMock.Setup(s => s.GetSubscriptionByGUID(productIdForNewSubscription))
                    .Returns(subscription);

                var cartProductService = new CartProductsService(
                    dbContext,
                    classesServiceMock.Object,
                    subscriptionsServiceMock.Object);

                cartProductService.AddToCart(userIdForNewClass,
                                             productTypeClass,
                                             productIdForNewClass);

                cartProductService.AddToCart(userIdForNewSubscription,
                                             productTypeSubscription,
                                             productIdForNewSubscription);
            }
        }

        [Fact]
        public void AddToCart_BetterSubscription_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "AddToCart_BetterSubscription_ShouldSucceed")
               .Options;

            var userId = Guid.NewGuid().ToString();
            var productId = Guid.NewGuid().ToString();
            var productTypeSubscription = ProductType.Subscription;

            var classesServiceMock = new Mock<IClassesService>();
            var subscriptionsServiceMock = new Mock<ISubscriptionsService>();

            var productIdForBetterSubscription = Guid.NewGuid().ToString();

            var subscription = new Subscription
            {
                Guid = productId,
                Duration = new TimeSpan(100),
                Name = "nothing",
                IsDeleted = false,
                OrderInPage = 1,
                Type = SubscriptionType.Daily

            };

            var userSubscription = new UserSubscription
            {
                SubscriptionOrderId = Guid.NewGuid().ToString(),
                UserId = userId,
                SubscriptionId = productId,
            };
            
            var userProduct = new UserProduct
            {
                OrderItemId = Guid.NewGuid().ToString(),
                UserId = userId,
                ProductId = productId,
                IsInCart = true,
            };

            var betterSubscription = new Subscription
            {
                Guid = productIdForBetterSubscription,
                Duration = new TimeSpan(100),
                Name = "nothing",
                IsDeleted = false,
                OrderInPage = 4,
                Type = SubscriptionType.Daily

            };

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Subscriptions.Add(subscription);
                dbContext.Subscriptions.Add(betterSubscription);
                dbContext.UsersSubscriptions.Add(userSubscription);
                dbContext.UsersProducts.Add(userProduct);

                dbContext.SaveChanges();

                subscriptionsServiceMock.Setup(s => s.GetSubscriptionByGUID(productIdForBetterSubscription))
                    .Returns(betterSubscription);
                
                subscriptionsServiceMock.Setup(s => s.GetBestNotExpiredSubscription(userId))
                    .Returns(userSubscription);

                var cartProductService = new CartProductsService(
                    dbContext,
                    classesServiceMock.Object,
                    subscriptionsServiceMock.Object);

                cartProductService.AddToCart(userId,
                                             productTypeSubscription,
                                             productIdForBetterSubscription);

                var expectedUsersSubscriptionsCount = 2;
                var actualCount = dbContext.UsersProducts.Count();

                Assert.Equal(expectedUsersSubscriptionsCount, actualCount);
            }
        }
        
        [Fact]
        public void AddToCart_BetterSubscription_ShouldFail()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "AddToCart_BetterSubscription_ShouldFail")
               .Options;

            var userId = Guid.NewGuid().ToString();
            var productId = Guid.NewGuid().ToString();
            var productTypeSubscription = ProductType.Subscription;

            var classesServiceMock = new Mock<IClassesService>();
            var subscriptionsServiceMock = new Mock<ISubscriptionsService>();

            var productIdForBetterSubscription = Guid.NewGuid().ToString();

            var subscription = new Subscription
            {
                Guid = productId,
                Duration = new TimeSpan(100),
                Name = "nothing",
                IsDeleted = false,
                OrderInPage = 4,
                Type = SubscriptionType.Daily

            };

            var userSubscription = new UserSubscription
            {
                SubscriptionOrderId = Guid.NewGuid().ToString(),
                UserId = userId,
                SubscriptionId = productId,
            };
            
            var userProduct = new UserProduct
            {
                OrderItemId = Guid.NewGuid().ToString(),
                UserId = userId,
                ProductId = productId,
                IsInCart = true,
            };

            var betterSubscription = new Subscription
            {
                Guid = productIdForBetterSubscription,
                Duration = new TimeSpan(100),
                Name = "nothing",
                IsDeleted = false,
                OrderInPage = 1,
                Type = SubscriptionType.Daily

            };

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Subscriptions.Add(subscription);
                dbContext.Subscriptions.Add(betterSubscription);
                dbContext.UsersSubscriptions.Add(userSubscription);
                dbContext.UsersProducts.Add(userProduct);

                dbContext.SaveChanges();

                subscriptionsServiceMock.Setup(s => s.GetSubscriptionByGUID(productIdForBetterSubscription))
                    .Returns(betterSubscription);
                
                subscriptionsServiceMock.Setup(s => s.GetBestNotExpiredSubscription(userId))
                    .Returns(userSubscription);

                var cartProductService = new CartProductsService(
                    dbContext,
                    classesServiceMock.Object,
                    subscriptionsServiceMock.Object);

                Assert.Throws<ArgumentException>(() => cartProductService.AddToCart(userId,
                                             productTypeSubscription,
                                             productIdForBetterSubscription));
            }
        }
        
        [Fact]
        public void BuyProducts_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "BuyProducts_ShouldSucceed")
               .Options;

            var userId = Guid.NewGuid().ToString();

            var productIdForNewClass = Guid.NewGuid().ToString();
            var productIdForNewSubscription = Guid.NewGuid().ToString();

            var productTypeClass = ProductType.Class;
            var productTypeSubscription = ProductType.Subscription;

            var classesServiceMock = new Mock<IClassesService>();

            var subscriptionsServiceMock = new Mock<ISubscriptionsService>();

            var classs = new Class
            {
                Guid = productIdForNewClass,
                Name = "nothing",
                IsDeleted = false,
                Address = "address",
                AllSeats = 100,
                Description = "Description",
                DurationInMinutes = 100,
                StartingDateTime = DateTime.Now,
            };

            var subscription = new Subscription
            {
                Guid = productIdForNewSubscription,
                Duration = new TimeSpan(100),
                Name = "nothing",
                IsDeleted = false,
                OrderInPage = 1,
                Type = SubscriptionType.Daily
            };

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Classes.Add(classs);
                dbContext.Subscriptions.Add(subscription);

                dbContext.Products.Add(
                    new Product
                    {
                        Guid = productIdForNewClass,
                        Name = "name",
                        IsDeleted = false,
                        Price = 1,
                        ProductType = ProductType.Class
                    });

                dbContext.Products.Add(
                    new Product
                    {
                        Guid = productIdForNewSubscription,
                        Name = "name",
                        IsDeleted = false,
                        Price = 1,
                        ProductType = ProductType.Subscription
                    });

                dbContext.Users.Add(new ApplicationUser
                {
                    Id = userId,
                    FirstName = "FirstName",
                    LastName = "LastName",
                });

                dbContext.SaveChanges();

                classesServiceMock.Setup(s => s.GetClassByGUID(productIdForNewClass))
                        .Returns(classs);

                classesServiceMock.Setup(c => c.CheckClassExists(productIdForNewClass))
                    .Returns(CheckClassExists(dbContext, productIdForNewClass));

                classesServiceMock.Setup(c => c.AddUserToClass(userId, productIdForNewClass))
                    .Callback(() => ClassesServiceAddUserToClass(dbContext, userId, productIdForNewClass));

                subscriptionsServiceMock.Setup(s => s.GetSubscriptionByGUID(productIdForNewSubscription))
                    .Returns(subscription);

                subscriptionsServiceMock.Setup(s => s.CheckSubscriptionExists(productIdForNewSubscription))
                    .Returns(CheckSubscriptionExists(dbContext, productIdForNewSubscription));

                subscriptionsServiceMock.Setup(s => s.AddSubscriptionToUser(userId, productIdForNewSubscription))
                    .Callback(() => AddSubscriptionToUser(dbContext, userId, productIdForNewSubscription));

                var cartProductService = new CartProductsService(
                    dbContext,
                    classesServiceMock.Object,
                    subscriptionsServiceMock.Object);

                cartProductService.AddToCart(userId,
                                             productTypeClass,
                                             productIdForNewClass);

                cartProductService.AddToCart(userId,
                                             productTypeSubscription,
                                             productIdForNewSubscription);

                var productsInCart = dbContext.UsersProducts
                                                .Where(up => up.UserId == userId && up.IsInCart)
                                                .ToList();

                //Act
                cartProductService.BuyProducts(userId);

                var userClasses = dbContext.UsersClasses.Where(uc => uc.UserId == userId).ToList();
                var usersSubscriptions = dbContext.UsersSubscriptions.Where(us => us.UserId == userId).ToList();

                Assert.True(productsInCart.All(p => p.IsInCart == false));
                Assert.Equal(1, userClasses.Count);
                Assert.Equal(1, usersSubscriptions.Count);
            }
        }

        [Fact]
        public void GetAllProductsForUser_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "GetAllProductsForUser_ShouldSucceed")
               .Options;

            var userId = Guid.NewGuid().ToString();

            var productIdForNewClass = Guid.NewGuid().ToString();
            var productIdForNewSubscription = Guid.NewGuid().ToString();

            var productTypeClass = ProductType.Class;
            var productTypeSubscription = ProductType.Subscription;

            var classesServiceMock = new Mock<IClassesService>();
            var subscriptionsServiceMock = new Mock<ISubscriptionsService>();

            var subscription = new Subscription
            {
                Guid = productIdForNewSubscription,
                Duration = new TimeSpan(100),
                Name = "nothing",
                IsDeleted = false,
                OrderInPage = 1,
                Type = SubscriptionType.Daily
            };

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Classes.Add(new Class 
                {
                    Guid = productIdForNewClass,
                    Description = "description",
                    Address = "address",
                    Name = "name",
                    DurationInMinutes = 60,
                    StartingDateTime = DateTime.Now,
                    AllSeats = 5
                });

                dbContext.Products.Add(
                    new Product
                    {
                        Guid = productIdForNewClass,
                        Name = "name",
                        IsDeleted = false,
                        Price = 1,
                        ProductType = ProductType.Class
                    });

                dbContext.Products.Add(
                    new Product
                    {
                        Guid = productIdForNewSubscription,
                        Name = "name",
                        IsDeleted = false,
                        Price = 1,
                        ProductType = ProductType.Subscription
                    });

                dbContext.Users.Add(new ApplicationUser
                {
                    Id = userId,
                    FirstName = "FirstName",
                    LastName = "LastName",
                });

                dbContext.Subscriptions.Add(subscription);

                dbContext.SaveChanges();

                subscriptionsServiceMock.Setup(s => s.GetSubscriptionByGUID(productIdForNewSubscription))
                    .Returns(subscription);

                var cartProductService = new CartProductsService(
                    dbContext,
                    classesServiceMock.Object,
                    subscriptionsServiceMock.Object);

                cartProductService.AddToCart(userId,
                             productTypeClass,
                             productIdForNewClass);

                cartProductService.AddToCart(userId,
                                             productTypeSubscription,
                                             productIdForNewSubscription);

                Assert.True(cartProductService.GetAllProductsForTheUser(userId).Count == 2);
            }

        }

        [Fact]
        public void RemoveFromCart_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                   .UseInMemoryDatabase(databaseName: "RemoveFromCart_ShouldSucceed")
                   .Options;

            var userId = Guid.NewGuid().ToString();

            var productIdForNewClass = Guid.NewGuid().ToString();
            var productIdForNewSubscription = Guid.NewGuid().ToString();

            var productTypeClass = ProductType.Class;
            var productTypeSubscription = ProductType.Subscription;

            var classesServiceMock = new Mock<IClassesService>();
            var subscriptionsServiceMock = new Mock<ISubscriptionsService>();

            var subscription = new Subscription
            {
                Guid = productIdForNewSubscription,
                Duration = new TimeSpan(100),
                Name = "nothing",
                IsDeleted = false,
                OrderInPage = 1,
                Type = SubscriptionType.Daily
            };

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Classes.Add(new Class
                {
                    Guid = productIdForNewClass,
                    Description = "description",
                    Address = "address",
                    Name = "name",
                    DurationInMinutes = 60,
                    StartingDateTime = DateTime.Now,
                    AllSeats = 5
                });

                dbContext.Products.Add(
                    new Product
                    {
                        Guid = productIdForNewClass,
                        Name = "name",
                        IsDeleted = false,
                        Price = 1,
                        ProductType = ProductType.Class
                    });

                dbContext.Products.Add(
                    new Product
                    {
                        Guid = productIdForNewSubscription,
                        Name = "name",
                        IsDeleted = false,
                        Price = 1,
                        ProductType = ProductType.Subscription
                    });

                dbContext.Users.Add(new ApplicationUser
                {
                    Id = userId,
                    FirstName = "FirstName",
                    LastName = "LastName",
                });

                dbContext.Subscriptions.Add(subscription);

                dbContext.SaveChanges();

                subscriptionsServiceMock.Setup(s => s.GetSubscriptionByGUID(productIdForNewSubscription))
                    .Returns(subscription);

                var cartProductService = new CartProductsService(
                    dbContext,
                    classesServiceMock.Object,
                    subscriptionsServiceMock.Object);

                cartProductService.AddToCart(userId,
                             productTypeClass,
                             productIdForNewClass);

                cartProductService.AddToCart(userId,
                                             productTypeSubscription,
                                             productIdForNewSubscription);

                cartProductService.RemoveFromCart(userId, productIdForNewClass);

                Assert.True(cartProductService.GetAllProductsForTheUser(userId).Count == 1);
            }
        }

        private void ClassesServiceAddUserToClass(ApplicationDbContext db, string userId, string classId)
        {
            if (!db.Users.Any(u => u.Id == userId) || !CheckClassExists(db, classId))
            {
                throw new ArgumentException("Invalid data.");
            }

            db.UsersClasses.Add(new UserClass
            {
                ClassId = classId,
                UserId = userId
            });

            db.SaveChanges();
        }

        private bool CheckClassExists(ApplicationDbContext db, string classId)
        {
            return db.Classes.Any(x => x.Guid == classId);
        }

        private bool CheckSubscriptionExists(ApplicationDbContext db, string subId)
        {
            return db.Subscriptions.Any(s => subId == s.Guid && !s.IsDeleted);
        }

        private void AddSubscriptionToUser(ApplicationDbContext db, string userId, string subId)
        {
            if (!db.Users.Any(u => u.Id == userId) || !CheckSubscriptionExists(db, subId))
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
    }
}