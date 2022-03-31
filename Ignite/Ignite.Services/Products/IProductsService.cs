using Ignite.Models;


namespace Ignite.Services.Products
{
    public interface IProductsService
    {
        Product GetProductByGUID(string productId);

        bool CheckProductExist(string productId);
    }
}
