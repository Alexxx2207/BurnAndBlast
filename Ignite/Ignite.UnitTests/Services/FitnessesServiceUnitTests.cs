using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ignite.Data;
using Ignite.Models;
using Ignite.Models.InputModels.Fitnesses;
using Ignite.Services.Fitnesses;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Ignite.UnitTests.Services
{
    public class FitnessesServiceUnitTests
    {
        [Fact]
        public void AddFitness_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "AddFitness_ShouldSucceed")
              .Options;

            var model = new FitnessesInputModel
            {
                Name = "name",
                Address = "address"
            };

            var expectedCount = 1;

            using (var dbContext = new ApplicationDbContext(options))
            {
                var fitnessService = new FitnessService(dbContext);

                fitnessService.AddFitness(model);

                Assert.Equal(expectedCount, dbContext.Fitnesses.Count());
            }
        }

        [Fact]
        public void CheckFitnessExist_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "CheckFitnessExist_ShouldSucceed")
              .Options;

            var fitnessId = Guid.NewGuid().ToString();

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Fitnesses.Add(new Fitness
                {
                    Guid = fitnessId,
                    Name = "name",
                    Address = "address",
                    IsDeleted = false,
                });

                dbContext.SaveChanges();

                var fitnessService = new FitnessService(dbContext);

                Assert.True(fitnessService.CheckFitnessExist(fitnessId));
            }
        }

        [Fact]
        public void GetAllFitnesses_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "GetAllFitnesses_ShouldSucceed")
              .Options;

            var expectedCount = 2;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Fitnesses.Add(new Fitness
                {
                    Guid = Guid.NewGuid().ToString(),
                    Name = "name",
                    Address = "address",
                    IsDeleted = false,
                });

                dbContext.Fitnesses.Add(new Fitness
                {
                    Guid = Guid.NewGuid().ToString(),
                    Name = "name",
                    Address = "address",
                    IsDeleted = false,
                });

                dbContext.SaveChanges();

                var fitnessService = new FitnessService(dbContext);

                Assert.Equal(expectedCount, fitnessService.GetAllFitnesses().Count());
            }
        }

        [Fact]
        public void IsNameAvailable_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "IsNameAvailable_ShouldSucceed")
                 .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Fitnesses.Add(new Fitness
                {
                    Guid = Guid.NewGuid().ToString(),
                    Name = "name",
                    Address = "address",
                });

                dbContext.SaveChanges();

                var fitnessService = new FitnessService(dbContext);

                Assert.True(fitnessService.IsNameAvailable("nameDifferent"));

                Assert.True(!fitnessService.IsNameAvailable("Name"));
                Assert.True(!fitnessService.IsNameAvailable("name"));
            }
        }

        [Fact]
        public void RemoveFitness_ShouldSucceed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "RemoveFitness_ShouldSucceed")
                .Options;

            var fitnessId = Guid.NewGuid().ToString();

            var expectedInDB = 1;
            var expectedFromService = 0;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Fitnesses.Add(new Fitness
                {
                    Guid = fitnessId,
                    Name = "name",
                    Address = "address",
                });

                dbContext.SaveChanges();

                var fitnessService = new FitnessService(dbContext);

                fitnessService.RemoveFitness(fitnessId);

                Assert.Equal(expectedInDB, dbContext.Fitnesses.Count());
                Assert.Equal(expectedFromService, fitnessService.GetAllFitnesses().Count);
            }

        }
    }
}
