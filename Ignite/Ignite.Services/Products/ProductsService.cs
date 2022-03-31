using Ignite.Data;
using Ignite.Models;


namespace Ignite.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext db;

        public ProductsService(
            ApplicationDbContext db)
        {
            this.db = db;
        }
        
        public bool CheckProductExist(string productId)
        {
            return db.Products.Any(x => x.Guid == productId);
        }

        public Product GetProductByGUID(string productId)
        {
            if (!CheckProductExist(productId))
                throw new ArgumentException("Invalid data.");

            return db.Products.First(p => p.Guid == productId);
        }
    }
}
