using Ignite.Models;
using Ignite.Models.Enums;
using Ignite.Models.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Services.Products
{
    public interface IProductsService
    {
        void AddToCart(string userId, ProductType productType ,string productId);

        void RemoveFromCart(string userId, string productId);

        void ClearCart(string userId);

        void BuyProducts(string userId);

        Product GetProductByGUID(string productId);

        bool CheckProductExist(string productId);
    }
}
