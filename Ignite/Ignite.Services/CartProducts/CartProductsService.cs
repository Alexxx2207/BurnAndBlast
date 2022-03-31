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
            if (CheckProductIsInCart(userId, productId))
            {
                throw new ArgumentException("Already in cart.");
            }

            DateTime? expiration = null;

            if (productType == ProductType.Subscription)
            {
                var sub = subscriptionsService.GetSubscriptionByGUID(productId);

                expiration = DateTime.Now + sub.Duration;
            }

            db.UsersProducts.Add(new UserProduct
            {
                OrderItemId = Guid.NewGuid().ToString(),
                UserId = userId,
                ProductId = productId,
                IsInCart = true,
                ExpirationDate = expiration
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

        public List<ProductInCartViewModel> GeAllProductsForTheUser(string userId)
        {
            return db.UsersProducts
                    .Where(up => up.UserId == userId && up.IsInCart)
                    .Select(up => new ProductInCartViewModel
                    {
                        GUID = up.ProductId,
                        Name = up.Product.Name,
                        Price = up.Product.Price,
                        ProductType = up.Product.ProductType,
                        ExpirationDate = up.ExpirationDate == null ?
                                          "Never" :
                                          up.ExpirationDate.Value.ToString("dd/MM/yyyy HH:mm"),
                    })
                    .ToList();
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
