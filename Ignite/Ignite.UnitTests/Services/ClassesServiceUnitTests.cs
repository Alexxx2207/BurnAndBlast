using Ignite.Data;
using Ignite.Models;
using Ignite.Models.InputModels.Classes;
using Ignite.Services.Classes;
using Ignite.Services.Products;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ignite.UnitTests.Services
{
    public class ClassesServiceUnitTests
    {
        [Fact]
        public void AddClasses_ShoudPass()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "AddClasses_ShoudPass")
            .Options;

            AddClassInputModel model = new AddClassInputModel
            {
                Name = "name",
                Address = "address",
                AllSeats = 100,
                Description = "description",
                DurationInMinutes = 60,
                Price = 100,
                StartingDateTime = DateTime.Now,
            };

            var expectedCount = 1;
            var actualClassesCount = 0;
            var actualProductsCount = 0;

            var productsServiceMock = new Mock<IProductsService>();

            using (var dbContext = new ApplicationDbContext(options))
            {
                var classesService = new ClassesService(dbContext, productsServiceMock.Object);
                classesService.AddClasses(model);

                actualClassesCount = dbContext.Classes.Count();
                actualProductsCount = dbContext.Products.Count();
            }

            Assert.Equal(expectedCount, actualClassesCount);
            Assert.Equal(expectedCount, actualProductsCount);
        }

        [Fact]
        public void AddUserToClass_InvalidUserId_ShouldFail()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
          .UseInMemoryDatabase(databaseName: "AddUserToClass_InvalidUserId_ShouldFail")
          .Options;

            var userId = Guid.NewGuid().ToString();
            var classId = Guid.NewGuid().ToString();

            var productsServiceMock = new Mock<IProductsService>();

            using (var dbContext = new ApplicationDbContext(options))
            {
                var classesService = new ClassesService(dbContext, productsServiceMock.Object);

                Assert.Throws<ArgumentException>(() => classesService.AddUserToClass(userId, classId));
            }
        }

        [Fact]
        public void AddUserToClass_ShouldPass()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(databaseName: "AddUserToClass_ShouldPass")
           .Options;

            var userId = Guid.NewGuid().ToString();
            var classId = Guid.NewGuid().ToString();

            var classs = new Class
            {
                Guid = classId,
                Name = "name",
                Address = "address",
                AllSeats = 100,
                Description = "description",
                DurationInMinutes = 60,
                StartingDateTime = DateTime.Now,
                IsDeleted = false,
            };

            var user = new ApplicationUser
            {
                Id = userId,
                FirstName = "FirstName",
                LastName = "LastName",
            };

            var productsServiceMock = new Mock<IProductsService>();

            var expectedCount = 1;
            var actualUserClassesCount = 0;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Users.Add(user);
                dbContext.Classes.Add(classs);

                dbContext.SaveChanges();

                var classesService = new ClassesService(dbContext, productsServiceMock.Object);


                classesService.AddUserToClass(userId, classId);

                actualUserClassesCount = dbContext.UsersClasses.Count();
            }

            Assert.Equal(expectedCount, actualUserClassesCount);
        }

        [Fact]
        public void ChangeClass_ShouldPass()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(databaseName: "ChangeClass_ShouldPass")
           .Options;

            var classId = Guid.NewGuid().ToString();

            var model = new ChangeClassInputModel
            {
                Guid = classId,
                Name = "changed",
                Address = "changed",
                AllSeats = 5,
                Description = "changed",
                DurationInMinutes = 5,
                Price = 5,
                StartingDateTime = DateTime.Now.AddDays(5),
            };

            var productsServiceMock = new Mock<IProductsService>();

            var productClass = new Product
            {
                Guid = classId,
                Name = "name",
                Price = 100,
                IsDeleted = false,
                ProductType = Models.Enums.ProductType.Class
            };

            var actualClass = new Class();
            var actualProduct = new Product();

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Classes.Add(new Class
                {
                    Guid = classId,
                    Name = "name",
                    Address = "address",
                    AllSeats = 100,
                    Description = "description",
                    DurationInMinutes = 60,
                    StartingDateTime = DateTime.Now,
                });

                dbContext.Products.Add(productClass);

                dbContext.SaveChanges();

                productsServiceMock.Setup(p => p.GetProductByGUID(classId))
                .Returns(productClass);

                var classesService = new ClassesService(dbContext, productsServiceMock.Object);

                classesService.ChangeClass(model);

                actualClass = dbContext.Find<Class>(classId);
                actualProduct = dbContext.Find<Product>(classId);
            }

            Assert.True(actualClass.Name == model.Name &&
                        actualClass.AllSeats == model.AllSeats &&
                        actualClass.DurationInMinutes == model.DurationInMinutes &&
                        actualClass.StartingDateTime == model.StartingDateTime &&
                        actualClass.Description == model.Description &&
                        actualClass.Address == model.Address &&
                        actualProduct.Name == model.Name &&
                        actualProduct.Price == model.Price);
        }

        [Fact]
        public void GetClasses_ShouldPass()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "GetClasses_ShouldPass")
              .Options;

            var productsServiceMock = new Mock<IProductsService>();

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Classes.Add(new Class
                {
                    Guid = Guid.NewGuid().ToString(),
                    Name = "name",
                    Address = "address",
                    AllSeats = 100,
                    Description = "description",
                    DurationInMinutes = 60,
                    StartingDateTime = DateTime.Now,
                });

                dbContext.Classes.Add(new Class
                {
                    Guid = Guid.NewGuid().ToString(),
                    Name = "name",
                    Address = "address",
                    AllSeats = 100,
                    Description = "description",
                    DurationInMinutes = 60,
                    StartingDateTime = DateTime.Now,
                });

                dbContext.SaveChanges();

                var classesService = new ClassesService(dbContext, productsServiceMock.Object);

                Assert.Equal(2, classesService.GetAllClasses(It.IsAny<string>()).Count());
            }
        }

        [Fact]
        public void GetClassByGUID_ShouldFail()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "GetClassByGUID_ShouldFail")
                 .Options;

            var productsServiceMock = new Mock<IProductsService>();

            var classId = Guid.NewGuid().ToString();
            var classIdDifferent = Guid.NewGuid().ToString();

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Classes.Add(new Class
                {
                    Guid = Guid.NewGuid().ToString(),
                    Name = "name",
                    Address = "address",
                    AllSeats = 100,
                    Description = "description",
                    DurationInMinutes = 60,
                    StartingDateTime = DateTime.Now,
                });

                dbContext.SaveChanges();

                var classesService = new ClassesService(dbContext, productsServiceMock.Object);

                Assert.Throws<ArgumentException>(() => classesService.GetClassByGUID(classIdDifferent));
            }
        }

        [Fact]
        public void GetDetailsOfAClass_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "GetDetailsOfAClass_ShouldSucceed")
                 .Options;

            var productsServiceMock = new Mock<IProductsService>();

            var classId = Guid.NewGuid().ToString();

            var expected = new
            {
                Guid = classId,
                Name = "name",
                Address = "address",
                AllSeats = 100,
                Description = "description",
                DurationInMinutes = 60,
                StartingDateTime = DateTime.Now,
                Price = 100m
            };

            var product = new Product
            {
                Name = expected.Name,
                Guid = expected.Guid,
                Price = expected.Price,
            };

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Classes.Add(new Class
                {
                    Guid = expected.Guid,
                    Name = expected.Name,
                    Address = expected.Address,
                    AllSeats = expected.AllSeats,
                    Description = expected.Description,
                    DurationInMinutes = expected.DurationInMinutes,
                    StartingDateTime = expected.StartingDateTime,
                });

                dbContext.Products.Add(product);

                dbContext.SaveChanges();

                productsServiceMock.Setup(p => p.GetProductByGUID(classId))
                    .Returns(product);

                var classesService = new ClassesService(dbContext, productsServiceMock.Object);

                var actualClass = classesService.GetDetailsOfClass(It.IsAny<string>(), classId);

                Assert.True(actualClass.Guid == expected.Guid &&
                        actualClass.Name == expected.Name &&
                        actualClass.AllSeats == expected.AllSeats &&
                        actualClass.DurationInMinutes == expected.DurationInMinutes &&
                        actualClass.StartingDateTime == expected.StartingDateTime &&
                        actualClass.Description == expected.Description &&
                        actualClass.Address == expected.Address &&
                        actualClass.Price == expected.Price.ToString("f2"));
            }
        }

        [Fact]
        public void GetTopClasses_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "GetTopClasses_ShouldSucceed")
                 .Options;

            var classIdTop1 = Guid.NewGuid().ToString();
            var classIdTop2 = Guid.NewGuid().ToString();
            var classIdTop3 = Guid.NewGuid().ToString();

            var productsServiceMock = new Mock<IProductsService>();

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Classes.Add(new Class
                {
                    Guid = classIdTop3,
                    Name = "top3",
                    Address = "address",
                    AllSeats = 100,
                    Description = "description",
                    DurationInMinutes = 60,
                    StartingDateTime = DateTime.Now,
                });

                dbContext.Classes.Add(new Class
                {
                    Guid = classIdTop1,
                    Name = "top1",
                    Address = "address",
                    AllSeats = 100,
                    Description = "description",
                    DurationInMinutes = 60,
                    StartingDateTime = DateTime.Now,
                });

                dbContext.Classes.Add(new Class
                {
                    Guid = classIdTop2,
                    Name = "top2",
                    Address = "address",
                    AllSeats = 100,
                    Description = "description",
                    DurationInMinutes = 60,
                    StartingDateTime = DateTime.Now,
                });

                dbContext.UsersClasses.Add(new UserClass
                {
                    ClassId = classIdTop1,
                    UserId = Guid.NewGuid().ToString()
                });

                dbContext.UsersClasses.Add(new UserClass
                {
                    ClassId = classIdTop1,
                    UserId = Guid.NewGuid().ToString()
                });

                dbContext.UsersClasses.Add(new UserClass
                {
                    ClassId = classIdTop1,
                    UserId = Guid.NewGuid().ToString()
                });

                dbContext.UsersClasses.Add(new UserClass
                {
                    ClassId = classIdTop2,
                    UserId = Guid.NewGuid().ToString()
                });

                dbContext.UsersClasses.Add(new UserClass
                {
                    ClassId = classIdTop2,
                    UserId = Guid.NewGuid().ToString()
                });

                dbContext.UsersClasses.Add(new UserClass
                {
                    ClassId = classIdTop3,
                    UserId = Guid.NewGuid().ToString()
                });

                dbContext.SaveChanges();

                var classesService = new ClassesService(dbContext, productsServiceMock.Object);

                var topClasses = classesService.GetTopClasses(It.IsAny<string>(), 3);

                Assert.True(topClasses[0].Name == "top1" &&
                            topClasses[1].Name == "top2" &&
                            topClasses[2].Name == "top3");
            }
        }

        [Fact]
        public void IsNameAvailable_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "IsNameAvailable_ShouldSucceed")
                 .Options;

            var productsServiceMock = new Mock<IProductsService>();

            var classId = Guid.NewGuid().ToString();

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Classes.Add(new Class
                {
                    Guid = classId,
                    Name = "name",
                    Address = "address",
                    AllSeats = 100,
                    Description = "description",
                    DurationInMinutes = 60,
                    StartingDateTime = DateTime.Now,
                });

                dbContext.SaveChanges();

                var classesService = new ClassesService(dbContext, productsServiceMock.Object);

                Assert.True(classesService.IsNameAvailable("nameDifferent", classId));
                Assert.True(classesService.IsNameAvailable("nameDifferent", Guid.NewGuid().ToString()));

                Assert.True(classesService.IsNameAvailable("name", classId));
                Assert.True(classesService.IsNameAvailable("Name", classId));

                Assert.True(!classesService.IsNameAvailable("Name", Guid.NewGuid().ToString()));
                Assert.True(!classesService.IsNameAvailable("name", Guid.NewGuid().ToString()));
            }
        }

        [Fact]
        public void RemoveClass_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "RemoveClass_ShouldSucceed")
                 .Options;

            var productsServiceMock = new Mock<IProductsService>();

            var classId = Guid.NewGuid().ToString();

            var product = new Product
            {
                Name = "name",
                Guid = classId,
                Price = 100,
            };

            var expectedInDB = 1;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Classes.Add(new Class
                {
                    Guid = classId,
                    Name = "name",
                    Address = "address",
                    AllSeats = 100,
                    Description = "description",
                    DurationInMinutes = 60,
                    StartingDateTime = DateTime.Now,
                });

                dbContext.Products.Add(product);

                dbContext.SaveChanges();

                productsServiceMock.Setup(p => p.GetProductByGUID(classId))
                    .Returns(product);

                var classesService = new ClassesService(dbContext, productsServiceMock.Object);

                classesService.RemoveClass(classId);

                Assert.Equal(expectedInDB, dbContext.Classes.Count());
                Assert.Empty(classesService.GetAllClasses(It.IsAny<string>()));
            }
        }

        [Fact]
        public void CheckUserAttends_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "CheckUserAttends_ShouldSucceed")
                 .Options;

            var productsServiceMock = new Mock<IProductsService>();

            var classId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();

            var product = new Product
            {
                Name = "name",
                Guid = classId,
                Price = 100,
            };

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Classes.Add(new Class
                {
                    Guid = classId,
                    Name = "name",
                    Address = "address",
                    AllSeats = 100,
                    Description = "description",
                    DurationInMinutes = 60,
                    StartingDateTime = DateTime.Now,
                });

                dbContext.Users.Add(new ApplicationUser
                { 
                    Id = userId,
                    FirstName = "FirstName",
                    LastName = "LastName",
                });

                dbContext.UsersClasses.Add(new UserClass
                { 
                    ClassId = classId,
                    UserId = userId,
                });

                dbContext.Products.Add(product);

                dbContext.SaveChanges();

                productsServiceMock.Setup(p => p.GetProductByGUID(classId))
                    .Returns(product);

                var classesService = new ClassesService(dbContext, productsServiceMock.Object);

                var actualClass = classesService.GetDetailsOfClass(userId, classId);

                Assert.True(actualClass.UserAttends == true);
            }
        }
    }
}
