using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ignite.Models.InputModels.Fitnesses;
using Ignite.Models.ParentModels;
using Ignite.Models.ViewModels.Fitnesses;
using Ignite.Services.Fitnesses;
using Ignite.Web.Areas.Administration.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Ignite.UnitTests.Controllers
{
    public class AdminFitnessesUnitTests
    {
        [Fact]
        public void AllFitnessesReturnViewCheck()
        {
            var fitnessService = new Mock<IFitnessService>();

            var parentModel = new AllFitnessParentModel()
            {
                FitnessesInputModel = new FitnessesInputModel(),
                GetFitnessViewModels = new List<GetFitnessViewModel>()
            };

            var adminFitnessesContoller = new AdminFitnessesController(fitnessService.Object);

            var viewResult = Assert.IsType<ViewResult>(adminFitnessesContoller.AllFitnesses());

            Assert.IsType<AllFitnessParentModel>(viewResult.ViewData.Model);
        }
        
        [Fact]
        public void AddFitnessReturnViewCheck()
        {
            var fitnessService = new Mock<IFitnessService>();

            string fitnessId = Guid.NewGuid().ToString();

            fitnessService.Setup(c => c.IsNameAvailable(fitnessId))
                .Returns(false);

            var parentModel = new AllFitnessParentModel()
            {
                FitnessesInputModel = new FitnessesInputModel()
                { 
                    Name = "name"
                },
                GetFitnessViewModels = new List<GetFitnessViewModel>()
            };

            var adminFitnessesContoller = new AdminFitnessesController(fitnessService.Object);

            var viewResult = Assert.IsType<ViewResult>(adminFitnessesContoller.AddFitness(parentModel));

            Assert.IsType<AllFitnessParentModel>(viewResult.ViewData.Model);
        }
        
        [Fact]
        public void AddFitnessReturnRedirectCheck()
        {
            var fitnessService = new Mock<IFitnessService>();

            string fitnessId = Guid.NewGuid().ToString();

            fitnessService.Setup(c => c.IsNameAvailable(fitnessId))
                .Returns(true);

            var parentModel = new AllFitnessParentModel()
            {
                FitnessesInputModel = new FitnessesInputModel()
                { 
                    Name = "name"
                },
                GetFitnessViewModels = new List<GetFitnessViewModel>()
            };

            var adminFitnessesContoller = new AdminFitnessesController(fitnessService.Object);

            Assert.IsType<ViewResult>(adminFitnessesContoller.AddFitness(parentModel));
        }
        
        [Fact]
        public void RemoveFitnesssReturnRedirectCheck()
        {
            var fitnessService = new Mock<IFitnessService>();

            string fitnessId = Guid.NewGuid().ToString();

            fitnessService.Setup(c => c.CheckFitnessExist(fitnessId))
                .Returns(true);

            var adminFitnessesContoller = new AdminFitnessesController(fitnessService.Object);

            Assert.IsType<RedirectResult>(adminFitnessesContoller.RemoveFitness(fitnessId));
        }
    }
}
