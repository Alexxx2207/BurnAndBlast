using Ignite.Data;
using Ignite.Models;
using Ignite.Models.Enums;
using Ignite.Models.ViewModels.Products;
using Ignite.Services.Classes;
using Ignite.Services.Subscriptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Services.CartProducts
{
    public class CartProductsService : ICartProductsService
    {
        private readonly ApplicationDbContext db;
        private readonly IClassesService classesService;
        private readonly ISubscriptionsService subscriptionsService;

        public CartProductsService(
            ApplicationDbContext db,
            IClassesService classesService,
            ISubscriptionsService subscriptionsService)
        {
            this.db = db;
            this.classesService = classesService;
            this.subscriptionsService = subscriptionsService;
        }

        public void AddToCart(string userId, ProductType productType, string productId)
        {
            if (CheckProductIsInCart(userId, productId) || 
                (db.UsersClasses.Count(uc => uc.ClassId == productId) >= db.Classes.Find(productId).AllSeats && productType == ProductType.Class))
            {
                throw new ArgumentException("Already in cart.");
            }

            if (productType == ProductType.Subscription)
            {
                var userbestSubscriptionOrderInPageObject = subscriptionsService
                                .GetBestNotExpiredSubscription(userId)?
                                .Subscription.OrderInPage;

                int bestUserSubscriptionOrderInPage = userbestSubscriptionOrderInPageObject == null ?
                                                        -1 : userbestSubscriptionOrderInPageObject.Value;

                int productIdOrder = subscriptionsService
                                        .GetSubscriptionByGUID(productId).OrderInPage;

                if (bestUserSubscriptionOrderInPage >= productIdOrder)
                { 
                    throw new ArgumentException("Invalid data.");
                }
            }

            db.UsersProducts.Add(new UserProduct
            {
                OrderItemId = Guid.NewGuid().ToString(),
                UserId = userId,
                ProductId = productId,
                IsInCart = true,
            });

            db.SaveChanges();
        }

        public void BuyProducts(string userId)
        {
            var userProducts = db.UsersProducts
                    .Where(x => x.UserId == userId && x.IsInCart)
                    .Include(x => x.Product)
                    .ToList();

            foreach (var userProduct in userProducts)
            {
                if (userProduct.Product.ProductType == ProductType.Class)
                {
                    classesService.AddUserToClass(userId, userProduct.Product.Guid);
                }
                else
                {
                    subscriptionsService.AddSubscriptionToUser(userId, userProduct.Product.Guid);
                }

                userProduct.IsInCart = false;
            }

            db.SaveChanges();
        }

        public bool CheckProductIsInCart(string userId, string productId)
        {
            return db.UsersProducts
                        .Any(x => x.ProductId == productId && x.UserId == userId && x.IsInCart);

        }

        public List<ProductInCartViewModel> GetAllProductsForTheUser(string userId)
        {
            var userProducts = db.UsersProducts
                    .Where(up => up.UserId == userId && up.IsInCart)
                    .Include(up => up.Product)
                    .ToList();

            var result = new List<ProductInCartViewModel>();

            foreach (var userProduct in userProducts)
            {
                int expirationAfterDays = -1;

                if (userProduct.Product.ProductType == ProductType.Subscription)
                {
                    expirationAfterDays = db.Subscriptions
                            .First(x =>
                                   x.Guid == userProduct.ProductId)
                            .Duration.Days;
                }
                result.Add(new ProductInCartViewModel
                {
                    GUID = userProduct.ProductId,
                    Name = userProduct.Product.Name,
                    Price = userProduct.Product.Price,
                    ProductType = userProduct.Product.ProductType,
                    ExpirationDate = expirationAfterDays == -1 ?
                                    "Never" 
                                    :
                                    expirationAfterDays.ToString(),
                });
            }

            return result;
        }
        public void RemoveFromCart(string userId, string productId)
        {
            var userProduct = db.UsersProducts
                .First(up => up.UserId == userId && up.ProductId == productId && up.IsInCart);

            userProduct.IsInCart = false;
            db.SaveChanges();
        }
    }
}
