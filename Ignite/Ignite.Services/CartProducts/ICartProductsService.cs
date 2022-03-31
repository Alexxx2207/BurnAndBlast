using Ignite.Models.Enums;
using Ignite.Models.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Services.CartProducts
{
    public interface ICartProductsService
    {
        void AddToCart(string userId, ProductType productType, string productId);

        void RemoveFromCart(string userId, string productId);

        void BuyProducts(string userId);

        bool CheckProductIsInCart(string userId, string productId);

        List<ProductInCartViewModel> GeAllProductsForTheUser(string userId);

    }
}
