using Ignite.Data;
using Ignite.Models;
using Ignite.Models.Enums;
using Ignite.Models.ViewModels.Products;
using Ignite.Services.Classes;
using Ignite.Services.Subscription_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext db;

        public ProductsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddToCart(string userId, ProductType productType, string productId)
        {
            throw new NotImplementedException();
        }

        public void BuyProducts(string userId)
        {
            throw new NotImplementedException();
        }

        public bool CheckProductExist(string productId)
        {
            return db.Products.Any(x => x.Guid == productId);
        }

        public void ClearCart(string userId)
        {
            throw new NotImplementedException();
        }

        public Product GetProductByGUID(string productId)
        {
            if (!CheckProductExist(productId))
                throw new ArgumentException("Invalid data.");

            return db.Products.First(p => p.Guid == productId);
        }

        public void RemoveFromCart(string userId, string productId)
        {
            throw new NotImplementedException();
        }
    }
}
