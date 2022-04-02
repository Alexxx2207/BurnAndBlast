using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ignite.Data;
using Ignite.Models;
using Ignite.Services.Products;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Ignite.UnitTests.Services
{
    public class ProductsServiceUnitTests
    {
        [Fact]
        public void CheckProductExist_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CheckProductExist_ShouldSucceed")
                .Options;

            var productId = Guid.NewGuid().ToString();

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Products.Add(new Product
                {
                    Guid = productId,
                    Name = "name",
                    Price = 100,
                    ProductType = Models.Enums.ProductType.Class,
                    IsDeleted = false,
                });

                dbContext.SaveChanges();

                var productsService = new ProductsService(dbContext);

                Assert.True(productsService.CheckProductExist(productId));
            }
        }

        [Fact]
        public void GetProductByGUID_ShouldFail()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetProductByGUID_ShouldFail")
                .Options;

            var productId = Guid.NewGuid().ToString();

            using (var dbContext = new ApplicationDbContext(options))
            {
                var productsService = new ProductsService(dbContext);

                Assert.Throws<ArgumentException>(() => productsService.GetProductByGUID(productId));
            }
        }

            [Fact]
        public void GetProductByGUID_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetProductByGUID_ShouldSucceed")
                .Options;

            var productId = Guid.NewGuid().ToString();

            var expectedProduct = new Product
            {
                Guid = productId,
                Name = "name",
                Price = 100,
                ProductType = Models.Enums.ProductType.Class,
                IsDeleted = false,
            };

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Products.Add(expectedProduct);

                dbContext.SaveChanges();

                var productsService = new ProductsService(dbContext);

                var actual = productsService.GetProductByGUID(productId);

                Assert.True(expectedProduct.Name == actual.Name &&
                            expectedProduct.Price == actual.Price &&
                            expectedProduct.ProductType == actual.ProductType);
            }
        }
    }
}
