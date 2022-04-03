using Ignite.Services.Users;
using Ignite.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ignite.UnitTests.Controllers
{
    public class UsersControllerUnitTests
    {
        [Fact]
        public void LoginReturnLocalRedirect()
        {
            var usersService = new Mock<IUsersService>();

            var usersController = new UsersController(usersService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            usersController.ControllerContext = context;

            var result = usersController.Login();

            Assert.IsType<LocalRedirectResult>(result);
        }

        [Fact]
        public void LoginRedirectBecauseUserIsLogged()
        {
            var usersService = new Mock<IUsersService>();

            var usersController = new UsersController(usersService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "identifier"),
                    new Claim(ClaimTypes.Name, "qa@qa.qa"),
                }, "TestAuthentication"));

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            usersController.ControllerContext = context;

            var result = usersController.Login();

            Assert.IsType<RedirectResult>(result);

        }
        
        [Fact]
        public void RegisterReturnLocalRedirect()
        {
            var usersService = new Mock<IUsersService>();

            var usersController = new UsersController(usersService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            usersController.ControllerContext = context;

            var result = usersController.Register();

            Assert.IsType<LocalRedirectResult>(result);
        }

        [Fact]
        public void RegisterRedirectBecauseUserIsLogged()
        {
            var usersService = new Mock<IUsersService>();

            var usersController = new UsersController(usersService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "identifier"),
                    new Claim(ClaimTypes.Name, "qa@qa.qa"),
                }, "TestAuthentication"));

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            usersController.ControllerContext = context;

            var result = usersController.Register();

            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public void RemoveImageReturnLocalRedirect()
        {
            var usersService = new Mock<IUsersService>();

            var usersController = new UsersController(usersService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            usersController.ControllerContext = context;

            var result = usersController.RemoveProfileImage(It.IsAny<string>());

            Assert.IsType<LocalRedirectResult>(result);
        }
    }
}
